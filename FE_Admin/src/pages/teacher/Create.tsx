import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";
import { yupResolver } from "@hookform/resolvers/yup";
import { Label } from "@/components/ui/label";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { FormField, FormItem } from "@/components/ui/form";

import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import DatePicker from "@/components/datepicker/DatePicker";
import { Input } from "@/components/ui/input";

import { convertDateISO } from "@/utils/utils";
import { ITeacher } from "@/types/teacher.type";
import { ITeacherSchema, teacherSchema } from "@/utils/schema/teacher.schema";
import {
	createTeacher,
	getTeacherDetail,
	updateTeacher,
} from "@/apis/teacher.api";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";

const initValues = {
	teacherId: "",
	fullName: "",
	dob: new Date(1990, 0, 1).toISOString(),
	gender: "",
	identificationNumber: "",
	avatar: "",
	level: "",
	phoneNumber: "",
	address: "",
	ethnic: "",
	timeStart: "",
	timeEnd: "",
};

const statusList = [
	{ value: "1", label: "Đang giảng dạy" },
	{ value: "2", label: "Tạm nghỉ" },
	{ value: "3", label: "Nghỉ việc" },
];

interface IPanelProps {
	type: "edit" | "create";
	selected?: ITeacher | null;
	disable: boolean;
	refreshData: (message: string) => void;
}
type IFormData = ITeacherSchema;
export function Panel(props: IPanelProps) {
	const { type, selected, disable, refreshData } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [file, setFile] = useState<File>();
	const [loading, setLoading] = useState<boolean>(false);
	const [status, setStatus] = useState<string>("");

	const {
		register,
		handleSubmit,
		formState: { errors },
		control,
		setValue,
		reset,
	} = useForm<IFormData>({
		defaultValues: initValues,
		resolver: yupResolver(teacherSchema),
	});

	const onSubmit = handleSubmit((data) => {
		const formData = new FormData();

		file && formData.append("avatar", file);
		formData.append(
			"teacherId", data?.teacherId
			// Math.floor(Math.random() * 1000).toString()
		);
		formData.append("fullName", data?.fullName);
		formData.append("dob", new Date(data?.dob).toISOString());
		data?.identificationNumber &&
			formData.append("identificationNumber", data?.identificationNumber);
		formData.append("gender", data?.gender);
		formData.append("address", data?.address);
		formData.append("ethnic", data?.ethnic);
		formData.append("phoneNumber", data?.phoneNumber);
		formData.append("email", data?.email);
		formData.append("level", data?.level);
		data?.timeEnd &&
			formData.append("timeEnd", new Date(data?.timeEnd).toISOString());
		formData.append("timeStart", new Date(data?.timeStart).toISOString());

		setLoading(true);

		(type === "create"
			? createTeacher(formData)
			: updateTeacher(formData, data?.teacherId as string)
		).then(() => {
			setLoading(false);
			setCount(0);
			reset(initValues);
			setOpenSheet(false);
			refreshData(
				type === "create"
					? "Thêm giáo viên thành công"
					: "Cập nhật thông tin giáo viên thành công"
			);
		});
	});

	const handleGetData = () => {
		if (count === 0 && type === "edit") {
			getTeacherDetail(selected?.teacherId as string).then((res) => {
				const info = res?.data?.data;
				setValue("teacherId", info?.teacherId);
				setValue("fullName", info?.fullName);
				setValue("dob", convertDateISO(info?.dob));
				setValue("gender", info?.gender);
				info?.identificationNumber &&
					setValue(
						"identificationNumber",
						info?.identificationNumber
					);
				setValue("ethnic", info?.ethnic);
				setValue("address", info?.address);
				setValue("phoneNumber", info?.phoneNumber);
				setValue("email", info?.email);
				setValue("level", info?.level);
				setValue("timeStart", convertDateISO(info?.timeStart));
				info?.timeEnd &&
					setValue("timeEnd", convertDateISO(info?.timeEnd));
				setStatus(info?.status ?? "");
				setCount((count) => count + 1);
			});
		}
	};

	useEffect(() => {
		setCount(0);
	}, [selected]);

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button disabled={disable} onClick={handleGetData}>
					{type === "edit" ? "Cập nhật" : "Thêm"}
				</Button>
			</SheetTrigger>
			<SheetContent className="max-h-screen overflow-y-scroll">
				<SheetHeader>
					<SheetTitle className="uppercase">
						{type === "edit"
							? "Cập nhật giáo viên"
							: "Thêm giáo viên"}
					</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
					<div>
						<Label htmlFor="teacherId" className="required">
							MSGV
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="teacherId"
							register={register}
							errorMessage={errors.teacherId?.message}
						/>
					</div>
					<div>
						<Label htmlFor="fullName" className="required">
							Họ tên
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="fullName"
							register={register}
							errorMessage={errors.fullName?.message}
						/>
					</div>
					<div>
						<Label htmlFor="dob" className="required">
							Ngày sinh
						</Label>
						<FormField
							control={control}
							name="dob"
							render={({ field }) => (
								<FormItem className="mt-[2px] flex flex-col">
									<DatePicker
										date={field.value}
										setDate={field.onChange}
										errorMessage={errors.dob?.message!}
									/>
								</FormItem>
							)}
						/>
					</div>
					<div>
						<Label htmlFor="gender" className="required">
							Giới tính
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="gender"
							register={register}
							errorMessage={errors.gender?.message}
						/>
					</div>
					<div>
						<Label htmlFor="identificationNumber">CCCD</Label>
						<CommonInput
							className="mt-[2px]"
							name="identificationNumber"
							register={register}
							// errorMessage={errors.identificationNumber?.message}
						/>
					</div>
					<div>
						<Label htmlFor="ethnic" className="required">
							Dân tộc
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="ethnic"
							register={register}
							errorMessage={errors.ethnic?.message}
						/>
					</div>
					<div>
						<Label htmlFor="address" className="required">
							Địa chỉ
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="address"
							register={register}
							errorMessage={errors.address?.message}
						/>
					</div>
					<div>
						<Label htmlFor="phoneNumber" className="required">
							Số điện thoại
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="phoneNumber"
							register={register}
							errorMessage={errors.phoneNumber?.message}
						/>
					</div>
					<div className="mb-4">
						<Label htmlFor="avatar">Avatar</Label>
						<Input
							className="mt-[2px]"
							id="avatar"
							type="file"
							name="avatar"
							onChange={(
								event: React.ChangeEvent<HTMLInputElement>
							) => setFile(event.target.files?.[0])}
						/>
					</div>
					<div>
						<Label htmlFor="email" className="required">
							Email
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="email"
							register={register}
							errorMessage={errors.email?.message}
						/>
					</div>
					<div>
						<Label htmlFor="level" className="required">
							Trình độ
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="level"
							register={register}
							errorMessage={errors.level?.message}
						/>
					</div>
					<div className="mb-5">
						<div className="mb-1 font-semibold">
							Tình trạng giảng dạy
						</div>
						<Select
							value={status}
							onValueChange={(value) => setStatus(value)}
						>
							<SelectTrigger className="">
								<SelectValue placeholder="Tình trạng giảng dạy" />
							</SelectTrigger>
							<SelectContent className="w-full">
								<SelectGroup>
									{statusList?.map((value) => (
										<SelectItem
											value={value.label}
											key={value.value}
										>
											{value.label}
										</SelectItem>
									))}
								</SelectGroup>
							</SelectContent>
						</Select>
					</div>

					<div>
						<Label htmlFor="timeStart" className="required">
							Ngày bắt đầu:
						</Label>
						<FormField
							control={control}
							name="timeStart"
							render={({ field }) => (
								<FormItem className="mt-[2px] flex flex-col">
									<DatePicker
										date={field.value}
										setDate={field.onChange}
										errorMessage={
											errors.timeStart?.message!
										}
									/>
								</FormItem>
							)}
						/>
					</div>
					<div>
						<Label htmlFor="timeEnd">Ngày kết thúc:</Label>
						<FormField
							control={control}
							name="timeEnd"
							render={({ field }) => (
								<FormItem className="mt-[2px] flex flex-col">
									<DatePicker
										date={field.value}
										setDate={field.onChange}
										// errorMessage={errors.timeEnd?.message!}
									/>
								</FormItem>
							)}
						/>
					</div>
					<Button
						className="w-full"
						onClick={onSubmit}
						loading={loading}
					>
						Lưu
					</Button>
				</div>
			</SheetContent>
		</Sheet>
	);
}
