import { createTimetable, getSubjectOfGrade } from "@/apis/timetable.api";
import { Button } from "@/components/ui/button";
import { FormField, FormItem } from "@/components/ui/form";
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
import { toast } from "@/components/ui/use-toast";
import { ISubject } from "@/types/assignment.type";
import {
	ITimetableSchema,
	timeTableSchema,
} from "@/utils/schema/timetable.schema";
import { yupResolver } from "@hookform/resolvers/yup";
import { useState } from "react";
import { useForm } from "react-hook-form";

const daysOfWeek = [
	{
		value: "2",
		name: "Thứ 2",
	},
	{
		value: "3",
		name: "Thứ 3",
	},
	{
		value: "4",
		name: "Thứ 4",
	},
	{
		value: "5",
		name: "Thứ 5",
	},
	{
		value: "6",
		name: "Thứ 6",
	},
	{
		value: "7",
		name: "Thứ 7",
	},
];

const periods = [
	{
		value: "1",
		name: "Tiết 1",
	},
	{
		value: "2",
		name: "Tiết 2",
	},
	{
		value: "3",
		name: "Tiết 3",
	},
	{
		value: "4",
		name: "Tiết 4",
	},
	{
		value: "5",
		name: "Tiết 5",
	},
];

interface IPanelProps {
	refreshData: (message: string) => void;
	grade: number;
	classId: string;
	semesterId: string;
}
const initValues = {
	period: "",
	subject: "",
	day: "",
};
export function CreateTimeTable(props: IPanelProps) {
	const { refreshData, grade, classId, semesterId } = props;
	const { reset, handleSubmit, control } = useForm<ITimetableSchema>({
		defaultValues: initValues,
		resolver: yupResolver(timeTableSchema),
	});

	const [openSheet, setOpenSheet] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);
	const [subject, setSubject] = useState<ISubject[]>([]);

	const handleGetData = () => {
		getSubjectOfGrade(grade).then((res) => {
			setSubject(res?.data?.data);
		});
	};

	const onSubmit = handleSubmit((data) => {
		setLoading(true);
		createTimetable({
			classId: classId,
			semesterId: semesterId,
			subjectId: Number(data?.subject),
			dayOfWeek: Number(data?.day),
			period: Number(data?.period),
		})
			.then(() => {
				setLoading(false);
				reset(initValues);
				setOpenSheet(false);
				refreshData("Thêm thời khoá biểu thành công");
			})
			.catch((err) => {
				toast({
					title: "Thông báo:",
					description: err?.response?.data?.message,
					variant: "destructive",
				});
				setLoading(false);
			});
	});

	const list = subject?.map((item) => ({
		label: item?.subjectName,
		value: item?.id,
	}));

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button onClick={handleGetData}>Thêm</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">Thêm</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
					<div className="mt-5">
						<Label htmlFor="subject" className="required ">
							Môn học:
						</Label>
						<FormField
							control={control}
							name="subject"
							render={({ field }) => (
								<FormItem className="mt-[2px] flex flex-col">
									<Select
										value={field.value}
										onValueChange={field.onChange}
									>
										<SelectTrigger className="w-full">
											<SelectValue placeholder="Chọn môn học" />
										</SelectTrigger>
										<SelectContent className="w-full">
											<SelectGroup>
												{list?.map((i) => (
													<SelectItem
														value={i.value.toString()}
														key={i.value}
													>
														{i.label}
													</SelectItem>
												))}
											</SelectGroup>
										</SelectContent>
									</Select>
								</FormItem>
							)}
						/>
					</div>
					<div className="mt-5">
						<Label htmlFor="day" className="required ">
							Thứ:
						</Label>
						<FormField
							control={control}
							name="day"
							render={({ field }) => (
								<FormItem className="mt-[2px] flex flex-col">
									<Select
										value={field.value}
										onValueChange={field.onChange}
									>
										<SelectTrigger className="w-full">
											<SelectValue placeholder="Chọn ngày học" />
										</SelectTrigger>
										<SelectContent className="w-full">
											<SelectGroup>
												{daysOfWeek?.map((i) => (
													<SelectItem
														value={i.value}
														key={i.value}
													>
														{i.name}
													</SelectItem>
												))}
											</SelectGroup>
										</SelectContent>
									</Select>
								</FormItem>
							)}
						/>
					</div>
					<div className="mt-5">
						<Label htmlFor="period" className="required ">
							Thứ:
						</Label>
						<FormField
							control={control}
							name="period"
							render={({ field }) => (
								<FormItem className="mt-[2px] flex flex-col">
									<Select
										value={field.value}
										onValueChange={field.onChange}
									>
										<SelectTrigger className="w-full">
											<SelectValue placeholder="Chọn tiết học" />
										</SelectTrigger>
										<SelectContent className="w-full">
											<SelectGroup>
												{periods?.map((i) => (
													<SelectItem
														value={i.value}
														key={i.value}
													>
														{i.name}
													</SelectItem>
												))}
											</SelectGroup>
										</SelectContent>
									</Select>
								</FormItem>
							)}
						/>
					</div>
				</div>

				<Button
					className="mt-8 w-full"
					onClick={onSubmit}
					loading={loading}
				>
					Lưu
				</Button>
			</SheetContent>
		</Sheet>
	);
}
