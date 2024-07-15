import { Button } from "@/components/ui/button";
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
import { useEffect, useState } from "react";
import { FormField } from "@/components/ui/form";
import {
	IMoveClassSchema,
	moveCLassSchema,
} from "@/utils/schema/assignment.schema";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { getClassFilter } from "@/apis/classroom.api";
import { updateAssignment } from "@/apis/asignment.api";
import { IAssignmentDisplay } from "@/types/assignment.type";

interface IPanelProps {
	selected?: IAssignmentDisplay;
	disable: boolean;
	refreshData: (value: string, varient?: boolean) => void;
	academicYear: string;
	grade: number;
}

const initValues = {
	classroom: "",
};

const MoveClass = (props: IPanelProps) => {
	const { selected, disable, refreshData, academicYear, grade } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);
	const [classroom, setClassroom] = useState<
		{ classId: string; className: string }[]
	>([]);

	const { handleSubmit, control, reset } = useForm<IMoveClassSchema>({
		defaultValues: initValues,
		resolver: yupResolver(moveCLassSchema),
	});

	const resetState = () => {
		setLoading(false);
		setCount(0);
		reset(initValues);
		setOpenSheet(false);
	};

	useEffect(() => {
		setCount(0);
		resetState();
	}, [selected, academicYear, grade]);

	const handleGetData = () => {
		if (count === 0) {
			getClassFilter(
				`${props.academicYear.slice(0, 4)} - ${
					Number(props.academicYear.slice(0, 4)) + 1
				}`,
				props.grade
			).then((res) => {
				setClassroom(res?.data.data);
			});

			setCount((count) => count + 1);
		}
	};

	const onSubmit = handleSubmit((data) => {
		setLoading(true);
		updateAssignment(selected?.assignmentId!, data?.classroom).then(() => {
			refreshData("Chuyển lớp thành công!");
		});
	});

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button disabled={disable} onClick={handleGetData}>
					Chuyển lớp
				</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">Chuyển lớp</SheetTitle>
				</SheetHeader>

				<div className="mt-5">
					<Label htmlFor="classroom" className="required">
						Chuyển lớp
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
};

export default MoveClass;
