import { createSubject, updateSubject } from "@/apis/asignment.api";
import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { ISubject } from "@/types/assignment.type";
import { ISubjectSchema, subjectSchema } from "@/utils/schema/subject.schema";
import { yupResolver } from "@hookform/resolvers/yup";
import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	type: "edit" | "create";
	selected?: ISubject | null;
	disable: boolean;
	refreshData: (value: string, varient?: boolean) => void;
}

const initValues = {
	grade: "",
	subjectName: "",
	subjectDescription: "",
};
export default function Create(props: IPanelProps) {
	const { type, selected, disable, refreshData } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);

	const {
		register,
		handleSubmit,
		formState: { errors },
		setValue,
		reset,
	} = useForm<ISubjectSchema>({
		defaultValues: initValues,
		resolver: yupResolver(subjectSchema),
	});

	useEffect(() => {
		reset(initValues);
		setCount(0);
	}, [selected]);

	const handleGetData = () => {
		if (count === 0 && type === "edit") {
			setValue("grade", selected?.grade?.toString()!);
			setValue("subjectName", selected?.subjectName!);
			selected?.description &&
				setValue("subjectDescription", selected?.description);
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
			subjectName: data?.subjectName,
			description: data?.subjectDescription!,
			grade: Number(data?.grade),
		};
		setLoading(true);
		if (type === "create") {
			createSubject(body).then(() => {
				resetState();
				refreshData("Thêm môn học thành công!");
			});
		} else {
			updateSubject(body, selected?.id!)
				.then(() => {
					resetState();
					refreshData("Cập nhật môn học thành công!");
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
						{type === "edit" ? "Cập nhật môn học" : "Thêm môn học"}
					</SheetTitle>
				</SheetHeader>

				<div className="mt-5">
					<Label htmlFor="grade" className="required">
						Khối:
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="grade"
						register={register}
						errorMessage={errors.grade?.message}
						placeholder="Ví dụ: 10"
					/>
				</div>
				<div>
					<Label htmlFor="subjectName" className="required">
						Tên môn học:
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="subjectName"
						register={register}
						errorMessage={errors.subjectName?.message}
						placeholder="Ví dụ: Toán 10"
					/>
				</div>
				<div>
					<Label htmlFor="subjectDescription">Mô tả:</Label>
					<CommonInput
						className="mt-[2px]"
						name="subjectDescription"
						register={register}
						placeholder="Ví dụ: 2023 - 2024"
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
