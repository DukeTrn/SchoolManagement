import {
	addTeacherDepartment,
	getTeacherDepartment,
	promoteTeacherDepartment,
} from "@/apis/department.api";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
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
	IDepartmentDetailSchema,
	departmentDetailSchema,
} from "@/utils/schema/department.schema";
import { yupResolver } from "@hookform/resolvers/yup";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";

interface IPanelProps {
	refreshData: (message: string) => void;
	departmentId: string;
	type: "edit" | "create";
	disable: boolean;
}
const initValues = {
	teacher: [],
};
export function Create(props: IPanelProps) {
	const { type, departmentId, disable, refreshData } = props;
	const {
		reset,
		handleSubmit,
		formState: { errors },
		control,
	} = useForm<IDepartmentDetailSchema>({
		defaultValues: initValues,
		resolver: yupResolver(departmentDetailSchema),
	});

	const [openSheet, setOpenSheet] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);
	const [teachers, setTeachers] = useState<
		{
			teacherId: string;
			fullName: string;
		}[]
	>([]);
	const [lead, setLead] = useState<string>("");
	const [sublead1, setSublead1] = useState<string>("");
	const [sublead2, setSublead2] = useState<string>("");

	useEffect(() => {
		setLead("");
		setSublead1("");
		setSublead2("");
	}, []);

	const handleGetData = () => {
		getTeacherDepartment(departmentId).then((res) => {
			setTeachers(res?.data?.data);
		});
	};

	const onSubmit = handleSubmit((data) => {
		setLoading(true);
		addTeacherDepartment({
			departmentId: departmentId,
			teacherIds: data?.teacher,
		}).then(() => {
			setLoading(false);
			reset(initValues);
			setOpenSheet(false);
			refreshData("Thêm giáo viên vào bộ môn thành công");
		});
	});

	const onEditSubmit = () => {
		setLoading(true);
		promoteTeacherDepartment({
			departmentId: departmentId,
			headId: lead,
			firstDeputyId: sublead1,
			secondDeputyId: sublead2,
		}).then(() => {
			setLoading(false);
			reset(initValues);
			setOpenSheet(false);
			refreshData("Thay đổi giáo viên");
		});
	};

	const list = teachers?.map((item) => ({
		label: item?.fullName,
		value: item?.teacherId,
	}));

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button disabled={disable} onClick={handleGetData}>
					{type === "edit" ? "Trưởng/phó bộ môn" : "Thêm"}
				</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">
						{type === "create" && "Thêm giáo viên"}
						{type === "edit" && "Trưởng/Phó bộ môn"}
					</SheetTitle>
				</SheetHeader>
				{type === "create" && (
					<div className="mb-8 mt-5">
						<Label htmlFor="teacher" className="required ">
							Giáo viên
						</Label>
						<FormField
							control={control}
							name="teacher"
							render={({ field }) => (
								<FormItem className="mt-[2px] flex flex-col">
									<MultiSelect
										options={list}
										onValueChange={field.onChange}
										// handleRetrieve={handleGetData}
										value={field.value}
										placeholder="Thêm giáo viên"
										variant="inverted"
										animation={2}
										maxCount={0}
										width={335}
										errorMessage={errors.teacher?.message!}
									/>
								</FormItem>
							)}
						/>
					</div>
				)}

				{type === "edit" && (
					<div>
						<div className="mb-8 mt-5">
							<Label htmlFor="lead" className="required">
								Trưởng bộ môn
							</Label>
							<div className="mb-5 mt-2 flex">
								<Select
									value={lead}
									onValueChange={(value) => setLead(value)}
								>
									<SelectTrigger className="">
										<SelectValue placeholder="Chọn giáo viên" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{list
												?.filter(
													(item) =>
														item.value !==
															sublead1 &&
														item.value !== sublead2
												)
												.map((i) => (
													<SelectItem
														value={i.value}
														key={i.label}
													>
														{i.label}
													</SelectItem>
												))}
										</SelectGroup>
										<Button
											className="w-full px-2"
											variant="secondary"
											size="sm"
											onClick={(e) => {
												e.stopPropagation();
												setLead("");
											}}
										>
											Hoàn tác
										</Button>
									</SelectContent>
								</Select>
							</div>
						</div>
						<div className="mb-8 mt-5">
							<Label htmlFor="sublead1" className="required ">
								Phó bộ môn 1
							</Label>
							<div className="mb-5 mt-2 flex">
								<Select
									value={sublead1}
									onValueChange={(value) =>
										setSublead1(value)
									}
								>
									<SelectTrigger className="">
										<SelectValue placeholder="Chọn giáo viên" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{list
												?.filter(
													(item) =>
														item.value !== lead &&
														item.value !== sublead2
												)
												.map((i) => (
													<SelectItem
														value={i.value}
														key={i.label}
													>
														{i.label}
													</SelectItem>
												))}
										</SelectGroup>
										<Button
											className="w-full px-2"
											variant="secondary"
											size="sm"
											onClick={(e) => {
												e.stopPropagation();
												setSublead1("");
											}}
										>
											Hoàn tác
										</Button>
									</SelectContent>
								</Select>
							</div>
						</div>
						<div className="mb-8 mt-5">
							<Label htmlFor="sublead2" className="required ">
								Phó bộ môn 2
							</Label>
							<div className="mb-5 mt-2 flex">
								<Select
									value={sublead2}
									onValueChange={(value) =>
										setSublead2(value)
									}
								>
									<SelectTrigger className="">
										<SelectValue placeholder="Chọn giáo viên" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{list
												?.filter(
													(item) =>
														item.value !== lead &&
														item.value !== sublead1
												)
												.map((i) => (
													<SelectItem
														value={i.value}
														key={i.label}
													>
														{i.label}
													</SelectItem>
												))}
										</SelectGroup>
										<Button
											className="w-full px-2"
											variant="secondary"
											size="sm"
											onClick={(e) => {
												e.stopPropagation();
												setSublead2("");
											}}
										>
											Hoàn tác
										</Button>
									</SelectContent>
								</Select>
							</div>
						</div>
					</div>
				)}
				<Button
					className="w-full"
					onClick={type === "create" ? onSubmit : onEditSubmit}
					loading={loading}
				>
					Lưu
				</Button>
			</SheetContent>
		</Sheet>
	);
}
