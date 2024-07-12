import {
	createClassroom,
	getFreeTeacher,
	updateClass,
} from "@/apis/classroom.api";
import { getAllAcademicYear } from "@/apis/semester.api";
import CommonInput from "@/components/input/CommonInput";
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
import { IClassroom } from "@/types/classroom.type";
import {
	IClassroomSchema,
	classroomSchema,
} from "@/utils/schema/classroom.schema";

import { yupResolver } from "@hookform/resolvers/yup";
import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	type: "edit" | "create";
	selected?: IClassroom | null;
	disable: boolean;
	refreshData: (value: string, varient?: boolean) => void;
}
interface IFreeTeacher {
	teacherId: string;
	fullName: string;
}
const initValues = {
	grade: "",
	className: "",
	academicYear: "",
	homeroomTeacherId: "",
};
export function Create(props: IPanelProps) {
	const { type, selected, disable, refreshData } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);
	const [freeTeacher, setFreeTeacher] = useState<IFreeTeacher[]>([]);
	const [academicYears, setAcademicYears] = useState<string[]>([]);

	const {
		control,
		register,
		handleSubmit,
		formState: { errors },
		setValue,
		reset,
		watch,
	} = useForm<IClassroomSchema>({
		defaultValues: initValues,
		resolver: yupResolver(classroomSchema),
	});
	const currentForm = watch();

	useEffect(() => {
		setCount(0);
	}, [selected]);

	useEffect(() => {
		getAllAcademicYear().then((res) => {
			setAcademicYears(res?.data?.data);
		});
	}, []);

	useEffect(() => {
		setFreeTeacher([]);
		if (currentForm?.academicYear && currentForm?.grade) {
			getFreeTeacher(currentForm?.academicYear).then((res) => {
				if (res?.data?.data.length > 0) {
					setFreeTeacher([
						...res.data.data,
						{
							teacherId: selected?.homeroomTeacherId!,
							fullName: selected?.homeroomTeacherName!,
						},
					]);
				} else {
					setFreeTeacher([
						{
							teacherId: selected?.homeroomTeacherId!,
							fullName: selected?.homeroomTeacherName!,
						},
					]);
				}
			});
		}
	}, [currentForm?.academicYear]);

	const handleGetData = () => {
		if (count === 0 && type === "edit") {
			setValue("className", selected?.className!);
			setValue("grade", String(selected?.grade!));
			setValue("academicYear", selected?.academicYear!);
			selected?.homeroomTeacherId &&
				setValue(
					"homeroomTeacherId",
					String(selected?.homeroomTeacherId)
				);
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
			className: data?.className,
			academicYear: data?.academicYear,
			grade: data?.grade,
			homeroomTeacherId: data?.homeroomTeacherId as string,
		};
		setLoading(true);
		if (type === "create") {
			createClassroom(body).then(() => {
				resetState();
				refreshData("Thêm lớp thành công!");
			});
		} else {
			updateClass(
				{
					className: data?.className,
					hoomroomTeacherId: data?.homeroomTeacherId as string,
				},
				selected?.classId!,
				data?.academicYear || currentForm?.academicYear
			)
				.then(() => {
					resetState();
					refreshData("Cập nhật lớp thành công!");
				})
				.catch((error) => {
					setLoading(false);
					refreshData(error?.response?.data?.error, true);
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
						{type === "edit" ? "Cập nhật lớp học" : "Thêm lớp học"}
					</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
					<Label htmlFor="grade" className="required">
						Khối:
					</Label>
					<FormField
						control={control}
						name="grade"
						render={({ field }) => (
							<Select
								value={field.value}
								onValueChange={field.onChange}
								disabled={type == "edit"}
							>
								<SelectTrigger className="w-full">
									<SelectValue placeholder="Chọn khối" />
								</SelectTrigger>
								<SelectContent className="w-full">
									<SelectGroup>
										<SelectItem value="10">
											Khối 10
										</SelectItem>
										<SelectItem value="11">
											Khối 11
										</SelectItem>
										<SelectItem value="12">
											Khối 12
										</SelectItem>
									</SelectGroup>
								</SelectContent>
							</Select>
						)}
					/>
					<div className="mt-1 min-h-[1.25rem] text-sm text-red-600">
						{errors?.grade?.message}
					</div>
				</div>
				<div>
					<Label htmlFor="className" className="required">
						Tên lớp học:
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="className"
						register={register}
						errorMessage={errors.className?.message}
						placeholder="Ví dụ: 10A1"
					/>
				</div>
				<div>
					<Label htmlFor="academicYear" className="required">
						Niên khoá:
					</Label>
					<FormField
						control={control}
						name="academicYear"
						render={({ field }) => (
							<Select
								value={field.value}
								onValueChange={field.onChange}
								disabled={type == "edit"}
							>
								<SelectTrigger className="w-full">
									<SelectValue placeholder="Chọn niên khoá" />
								</SelectTrigger>
								<SelectContent className="w-full">
									<SelectGroup>
										{academicYears?.map((value) => (
											<SelectItem
												value={value}
												key={value}
											>
												{value}
											</SelectItem>
										))}
									</SelectGroup>
								</SelectContent>
							</Select>
						)}
					/>
					<div className="mt-1 min-h-[1.25rem] text-sm text-red-600">
						{errors?.academicYear?.message}
					</div>
				</div>
				<div>
					<Label htmlFor="homeroomTeacherId">
						Giáo viên chủ nhiệm:
					</Label>
					<FormField
						control={control}
						name="homeroomTeacherId"
						render={({ field }) => (
							<Select
								value={field.value}
								onValueChange={field.onChange}
							>
								<SelectTrigger className="w-full">
									<SelectValue placeholder="Chọn giáo viên chủ nhiệm" />
								</SelectTrigger>
								<SelectContent className="w-full">
									<SelectGroup>
										{freeTeacher?.map((item) => (
											<SelectItem
												value={item.teacherId}
												key={item.teacherId}
											>
												{item.fullName}
											</SelectItem>
										))}
									</SelectGroup>
								</SelectContent>
							</Select>
						)}
					/>
				</div>

				<div className="mt-8">
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
