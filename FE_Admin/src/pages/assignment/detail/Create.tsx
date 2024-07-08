import { createAssignment } from "@/apis/asignment.api";
import { getClassFilter } from "@/apis/classroom.api";
import { getTeacherFilter } from "@/apis/teacher.api";
import { Button } from "@/components/ui/button";
import { FormField } from "@/components/ui/form";

import { Label } from "@/components/ui/label";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { IAssignmentDisplay } from "@/types/assignment.type";
import {
	IAssignmentSchema,
	assignmentSchema,
} from "@/utils/schema/assignment.schema";

import { yupResolver } from "@hookform/resolvers/yup";
import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	type: "edit" | "create";
	selected?: IAssignmentDisplay | null;
	disable: boolean;
	refreshData: (value: string) => void;
	academicYear: string;
	grade: number;
	semesterId: string;
	subjectId: string;
}
const initValues = {
	classroom: "",
	teacher: "",
};
export function Create(props: IPanelProps) {
	const {
		type,
		selected,
		disable,
		refreshData,
		academicYear,
		grade,
		semesterId,
		subjectId,
	} = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);
	const [classroom, setClassroom] = useState<
		{ classId: string; className: string }[]
	>([]);
	const [teachers, setTeachers] = useState<
		{ teacherId: string; fullName: string }[]
	>([]);

	const {
		handleSubmit,
		formState: { errors },
		control,
		reset,
	} = useForm<IAssignmentSchema>({
		defaultValues: initValues,
		resolver: yupResolver(assignmentSchema),
	});

	useEffect(() => {
		setCount(0);
		resetState();
	}, [selected, academicYear, grade]);

	const handleGetData = () => {
		if (count === 0) {
			Promise.all([
				getClassFilter(
					`${academicYear.slice(0, 4)} - ${
						Number(academicYear.slice(0, 4)) + 1
					}`,
					grade
				),
				getTeacherFilter(),
			]).then((res) => {
				setClassroom(res[0]?.data.data);
				setTeachers(res[1]?.data.data);
			});

			setCount((count) => count + 1);
		}
	};

	const resetState = () => {
		setLoading(false);
		setCount(0);
		reset(initValues);
		setOpenSheet(false);
	};

	const onSubmit = handleSubmit((data) => {
		const body = {
			semesterId: semesterId,
			teacherId: data?.teacher,
			subjectId: subjectId,
			classId: data?.classroom,
		};
		setLoading(true);
		if (type === "create") {
			createAssignment(body).then(() => {
				resetState();
				refreshData("Thêm giáo viên thành công!");
			});
		}
	});

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button disabled={disable} onClick={handleGetData}>
					{type === "edit" ? "Cập nhật" : "Thêm"}
				</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">
						{type === "edit"
							? "Cập nhật phân công giảng dạy"
							: "Thêm phân công giảng dạy"}
					</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
					<Label htmlFor="classroom" className="required">
						Lớp học:
					</Label>
					<div className="mt-1">
						<FormField
							control={control}
							name="classroom"
							render={({ field }) => (
								<Select
									value={field.value}
									onValueChange={field.onChange}
								>
									<SelectTrigger className="w-full">
										<SelectValue placeholder="Chọn lớp học" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{classroom?.map((i) => (
												<SelectItem
													value={i.classId}
													key={i.classId}
												>
													{i.className}
												</SelectItem>
											))}
										</SelectGroup>
									</SelectContent>
								</Select>
							)}
						/>
					</div>

					<div className="mt-1 min-h-[1.25rem] text-sm text-red-600">
						{errors?.classroom?.message}
					</div>
				</div>
				<div>
					<Label htmlFor="teacher" className="required">
						Giáo viên:
					</Label>
					<div className="mt-1">
						<FormField
							control={control}
							name="teacher"
							render={({ field }) => (
								<Select
									value={field.value}
									onValueChange={field.onChange}
								>
									<SelectTrigger className="w-full">
										<SelectValue placeholder="Chọn giáo viên" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{teachers?.map((i) => (
												<SelectItem
													value={i.teacherId}
													key={i.teacherId}
												>
													{i.fullName}
												</SelectItem>
											))}
										</SelectGroup>
									</SelectContent>
								</Select>
							)}
						/>
					</div>

					<div className="mt-1 min-h-[1.25rem] text-sm text-red-600">
						{errors?.teacher?.message}
					</div>
				</div>

				<div className="mt-3">
					<Button
						className="w-full"
						loading={loading}
						onClick={onSubmit}
					>
						Lưu
					</Button>
				</div>
			</SheetContent>
		</Sheet>
	);
}
