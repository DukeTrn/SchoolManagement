import { updateTimetable } from "@/apis/timetable.api";
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
import { ITimetable } from "@/types/timetable.type";
import {
	editTimeTableSchema,
	IEditTimetableSchema,
} from "@/utils/schema/timetable.schema";
import { yupResolver } from "@hookform/resolvers/yup";
import { SetStateAction, useState } from "react";
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
	openEditSheet: boolean;
	setOpenEditSheet: React.Dispatch<SetStateAction<boolean>>;
	setOpenSheet: React.Dispatch<SetStateAction<boolean>>;
	timetable: ITimetable;
}
const initValues = {
	period: "",
	day: "",
};
export function EditTimeTable(props: IPanelProps) {
	const {
		refreshData,
		openEditSheet,
		setOpenEditSheet,
		setOpenSheet,
		timetable,
	} = props;
	const { reset, handleSubmit, control } = useForm<IEditTimetableSchema>({
		defaultValues: initValues,
		resolver: yupResolver(editTimeTableSchema),
	});

	const [loading, setLoading] = useState<boolean>(false);

	const onSubmit = handleSubmit((data) => {
		setLoading(true);
		updateTimetable(
			{
				dayOfWeek: Number(data?.day),
				period: Number(data?.period),
			},
			timetable?.timetableId
		)
			.then(() => {
				setLoading(false);
				reset(initValues);
				setOpenEditSheet(false);
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

	return (
		<Sheet open={openEditSheet} onOpenChange={setOpenEditSheet}>
			<SheetTrigger asChild>
				<Button className="w-full">Cập nhật</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">
						Cập nhật thời khoá biểu
					</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
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
