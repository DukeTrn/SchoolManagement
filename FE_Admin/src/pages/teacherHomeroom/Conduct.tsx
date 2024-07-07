import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import {
	Select,
	SelectContent,
	SelectGroup,
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
import CommonInput from "@/components/input/CommonInput";

interface IPanelProps {
	selected?: any;
	disable: boolean;
	refreshData: (value: string, varient?: boolean) => void;
}

const initValues = {
	classroom: "",
};

const Conduct = (props: IPanelProps) => {
	const { selected, disable } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);

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
	}, [selected]);

	const handleGetData = () => {
		if (count === 0) {
			// getClassFilter(
			// 	`${props.academicYear.slice(0, 4)} - ${
			// 		Number(props.academicYear.slice(0, 4)) + 1
			// 	}`,
			// 	props.grade
			// ).then((res) => {
			// 	setClassroom(res?.data.data);
			// });

			setCount((count) => count + 1);
		}
	};

	const onSubmit = handleSubmit((data) => {
		console.log(data);
		setLoading(true);
		// updateAssignment(selected?.assignmentId, data?.classroom).then(() => {
		// 	refreshData("Chuyển lớp thành công!");
		// });
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
					<Label htmlFor="classroom" className="required">
						Học kỳ
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
										<SelectValue placeholder="20191" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{/* {classroom?.map((i) => (
												<SelectItem
													value={i.classId}
													key={i.classId}
												>
													{i.className}
												</SelectItem>
											))} */}
										</SelectGroup>
									</SelectContent>
								</Select>
							)}
						/>
					</div>
				</div>
				<div className="mt-5">
					<Label htmlFor="classroom" className="required">
						Hạnh kiểm
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
										<SelectValue placeholder="Tốt" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{/* {classroom?.map((i) => (
												<SelectItem
													value={i.classId}
													key={i.classId}
												>
													{i.className}
												</SelectItem>
											))} */}
										</SelectGroup>
									</SelectContent>
								</Select>
							)}
						/>
					</div>
				</div>

				<div className="mt-5">
					<Label htmlFor="email" className="required">
						Nhận xét
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="email"
						placeholder="Tốt"
						// register={register}
						// errorMessage={errors.email?.message}
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
