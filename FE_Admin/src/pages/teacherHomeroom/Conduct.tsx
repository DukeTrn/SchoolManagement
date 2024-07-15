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
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import CommonInput from "@/components/input/CommonInput";
import { IFilterSemesters } from "@/types/semester.type";
import { getSemesterInAcademicYear } from "@/apis/semester.api";
import {
	conductSchema,
	IConductSchema,
} from "@/utils/schema/teacher.info.schema";
import { getConductId, updateConduct } from "@/apis/conduct.api";

interface IPanelProps {
	selected?: any;
	disable: boolean;
	refreshData: (value: string, varient?: boolean) => void;
	academicYear: string;
}

const initValues = {};

const conductList = [
	{
		name: "",
		value: 0,
	},
	{
		name: "Tốt",
		value: 1,
	},
	{
		name: "Khá",
		value: 2,
	},
	{
		name: "Trung bình",
		value: 3,
	},
	{
		name: "Yếu",
		value: 4,
	},
];

const Conduct = (props: IPanelProps) => {
	const { selected, disable, refreshData, academicYear } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);
	const [semesters, setSemesters] = useState<IFilterSemesters[]>([]);

	const { handleSubmit, control, reset, register } = useForm<IConductSchema>({
		defaultValues: initValues,
		resolver: yupResolver(conductSchema),
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
	}, [selected]);

	const handleGetData = () => {
		if (count === 0) {
			getSemesterInAcademicYear(academicYear).then((res) => {
				setSemesters(res?.data.data);
			});
			setCount((count) => count + 1);
		}
	};

	const onSubmit = handleSubmit(async (data) => {
		setLoading(true);
		const conduct = await getConductId(selected?.studentId, data?.semester);
		await updateConduct(
			{ conductType: Number(data?.conduct), feedback: data?.comment },
			conduct?.data.conductId
		);
		setLoading(false);
		refreshData("Cập nhật thành công!");
	});

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button disabled={disable} onClick={handleGetData}>
					Hạnh kiểm
				</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">Hạnh kiểm</SheetTitle>
				</SheetHeader>

				<div className="mt-5">
					<Label htmlFor="semester" className="required">
						Học kỳ
					</Label>
					<div className="mt-1">
						<FormField
							control={control}
							name="semester"
							render={({ field }) => (
								<Select
									value={field.value}
									onValueChange={field.onChange}
								>
									<SelectTrigger className="w-full">
										<SelectValue placeholder="Chọn học kỳ" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{semesters?.map((i) => (
												<SelectItem
													value={i.semesterId}
													key={i.semesterId}
												>
													{i.semesterName}
												</SelectItem>
											))}
										</SelectGroup>
									</SelectContent>
								</Select>
							)}
						/>
					</div>
				</div>
				<div className="mt-5">
					<Label htmlFor="conduct" className="required">
						Hạnh kiểm
					</Label>
					<div className="mt-1">
						<FormField
							control={control}
							name="conduct"
							render={({ field }) => (
								<Select
									value={field.value}
									onValueChange={field.onChange}
								>
									<SelectTrigger className="w-full">
										<SelectValue placeholder="Chọn hạnh kiểm" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{conductList?.map((i) => (
												<SelectItem
													value={i.value.toString()}
													key={i.value}
												>
													{i.name}
												</SelectItem>
											))}
										</SelectGroup>
									</SelectContent>
								</Select>
							)}
						/>
					</div>
				</div>

				<div className="mt-5">
					<Label htmlFor="comment" className="required">
						Nhận xét
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="comment"
						placeholder=""
						register={register}
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
};

export default Conduct;
