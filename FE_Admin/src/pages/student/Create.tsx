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
import { FormField, FormItem, FormMessage } from "@/components/ui/form";

import { IStudent } from "@/types/student.type";
import { IStudentSchema, studentSchema } from "@/utils/schema/student.schema";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import DatePicker from "@/components/datepicker/DatePicker";
import { Input } from "@/components/ui/input";
import { createStudent, getStudentDetail } from "@/apis/student.api";
import { convertDateISO } from "@/utils/utils";

const initValues = {
	studentId: "",
	fullName: "",
	dob: new Date(1990, 0, 1).toISOString(),
	gender: "",
	phoneNumber: "",
	email: "",
	status: "",
	identificationNumber: "",
	address: "",
	ethnic: "",
	avatar: "",
	fatherName: "",
	fatherJob: "",
	fatherPhoneNumber: "",
	fatherEmail: "",
	motherName: "",
	motherJob: "",
	motherPhoneNumber: "",
	motherEmail: "",
	academicYear: "",
};
interface IPanelProps {
	type: "edit" | "create";
	selectedStudent?: IStudent | null;
	disable: boolean;
	refreshData: (message: string) => void;
}
type IFormData = IStudentSchema;
export function Panel(props: IPanelProps) {
	const { type, selectedStudent, disable, refreshData } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [file, setFile] = useState<File>();
	const [loading, setLoading] = useState<boolean>(false);

	const {
		register,
		handleSubmit,
		formState: { errors },
		control,
		setValue,
		reset,
	} = useForm<IFormData>({
		defaultValues: initValues,
		resolver: yupResolver(studentSchema),
	});

	const onSubmit = handleSubmit((data) => {
		const formData = new FormData();

		file && formData.append("avatar", file);
		formData.append(
			"studentId",
			Math.floor(Math.random() * 1000).toString()
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
		formData.append("fatherName", data?.fatherName);
		formData.append("fatherJob", data?.fatherJob);
		formData.append("fatherPhoneNumber", data?.fatherPhoneNumber);
		data?.fatherEmail && formData.append("fatherEmail", data?.fatherEmail);
		formData.append("motherName", data?.motherName);
		formData.append("motherJob", data?.motherJob);
		formData.append("motherPhoneNumber", data?.motherPhoneNumber);
		data?.motherEmail && formData.append("motherEmail", data?.motherEmail);

		setLoading(true);
		createStudent(formData).then(() => {
			setLoading(false);
			setCount(0);
			reset(initValues);
			setOpenSheet(false);
			refreshData(
				type === "create"
					? "Thêm học sinh thành công"
					: "Cập nhật thông tin học sinh thành công"
			);
		});
	});

	const handleGetData = () => {
		if (count === 0) {
			getStudentDetail(selectedStudent?.studentId as string).then(
				(res) => {
					const info = res?.data?.data;
					setValue("studentId", info?.studentId);
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
					info?.email && setValue("email", info?.email);
					setValue("fatherName", info?.fatherName);
					setValue("fatherJob", info?.fatherJob);
					setValue("fatherPhoneNumber", info?.fatherPhoneNumber);
					setValue("motherName", info?.motherName);
					setValue("motherJob", info?.motherJob);
					setValue("motherPhoneNumber", info?.motherPhoneNumber);
					setCount((count) => count + 1);
				}
			);
		}
	};

	useEffect(() => {
		setCount(0);
	}, [selectedStudent]);

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
							? "Cập nhật học sinh"
							: "Thêm học sinh"}
					</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
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
					<div>
						<Label htmlFor="email">Email</Label>
						<CommonInput
							className="mt-[2px]"
							name="email"
							register={register}
							// errorMessage={errors.email?.message}
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
						<Label htmlFor="fatherName" className="required">
							Họ tên bố:
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="fatherName"
							register={register}
							errorMessage={errors.fatherName?.message}
						/>
					</div>
					<div>
						<Label htmlFor="fatherJob" className="required">
							Nghề nghiệp bố:
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="fatherJob"
							register={register}
							errorMessage={errors.fatherJob?.message}
						/>
					</div>
					<div>
						<Label htmlFor="fatherPhoneNumber" className="required">
							SĐT bố:
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="fatherPhoneNumber"
							register={register}
							errorMessage={errors.fatherPhoneNumber?.message}
						/>
					</div>
					<div>
						<Label htmlFor="motherName" className="required">
							Họ tên mẹ:
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="motherName"
							register={register}
							errorMessage={errors.motherName?.message}
						/>
					</div>
					<div>
						<Label htmlFor="motherJob" className="required">
							Nghề nghiệp mẹ:
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="motherJob"
							register={register}
							errorMessage={errors.motherJob?.message}
						/>
					</div>
					<div>
						<Label htmlFor="motherPhoneNumber" className="required">
							SĐT mẹ:
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="motherPhoneNumber"
							register={register}
							errorMessage={errors.motherPhoneNumber?.message}
						/>
					</div>

					<Button
						className="w-full"
						onClick={onSubmit}
						loading={loading}
					>
						Save changes
					</Button>
				</div>
			</SheetContent>
		</Sheet>
	);
}
