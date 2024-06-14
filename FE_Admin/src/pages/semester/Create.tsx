import { createSemester, updateSemester } from "@/apis/semester.api";
import DatePicker from "@/components/datepicker/DatePicker";
import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";
import { FormField, FormItem } from "@/components/ui/form";
import { Label } from "@/components/ui/label";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import {
	ISemesterSchema,
	semesterSchema,
} from "@/utils/schema/semester.schema";
import { convertDateISO } from "@/utils/utils";
import { yupResolver } from "@hookform/resolvers/yup";
import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	type: "edit" | "create";
	selected?: ISemester | null;
	disable: boolean;
	refreshData: (value: string) => void;
}
const initValues = {
	semesterId: "",
	semesterName: "",
	timeStart: "",
	timeEnd: "",
};
export function Create(props: IPanelProps) {
	const { type, selected, disable, refreshData } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);

	const {
		control,
		register,
		handleSubmit,
		formState: { errors },
		setValue,
		reset,
	} = useForm<ISemesterSchema>({
		defaultValues: initValues,
		resolver: yupResolver(semesterSchema),
	});

	useEffect(() => {
		setCount(0);
	}, [selected]);

	const handleGetData = () => {
		if (count === 0 && type === "edit") {
			setValue("semesterId", selected?.semesterId!);
			setValue("semesterName", selected?.semesterName!);
			setValue("timeStart", convertDateISO(selected?.timeStart!));
			selected?.timeEnd &&
				setValue("timeEnd", convertDateISO(selected?.timeEnd));
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
			semesterId: data?.semesterId,
			semesterName: data?.semesterName,
			timeStart: new Date(data?.timeStart).toISOString(),
			timeEnd: new Date(data?.timeEnd as string).toISOString(),
		};
		setLoading(true);
		if (type === "create") {
			createSemester(body).then(() => {
				resetState();
				refreshData("Thêm học kỳ thành công!");
			});
		} else {
			updateSemester(body).then(() => {
				resetState();
				refreshData("Cập nhật học kỳ thành công!");
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
						{type === "edit" ? "Cập nhật học kỳ" : "Thêm học kỳ"}
					</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
					<Label htmlFor="semesterId" className="required">
						Mã hoc kỳ:
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="semesterId"
						register={register}
						errorMessage={errors.semesterId?.message}
						disabled={type === "edit"}
					/>
				</div>
				<div>
					<Label htmlFor="semesterName" className="required">
						Tên học kỳ:
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="semesterName"
						register={register}
						errorMessage={errors.semesterName?.message}
					/>
				</div>
				<div>
					<Label htmlFor="timeStart" className="required">
						Thời gian bắt đầu:
					</Label>
					<FormField
						control={control}
						name="timeStart"
						render={({ field }) => (
							<FormItem className="mt-[2px] flex flex-col">
								<DatePicker
									date={field.value}
									setDate={field.onChange}
									errorMessage={errors.timeStart?.message!}
								/>
							</FormItem>
						)}
					/>
				</div>
				<div>
					<Label htmlFor="timeEnd">Thời gian kết thúc:</Label>
					<FormField
						control={control}
						name="timeEnd"
						render={({ field }) => (
							<FormItem className="mt-[2px] flex flex-col">
								<DatePicker
									date={field.value}
									setDate={field.onChange}
								/>
							</FormItem>
						)}
					/>
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
