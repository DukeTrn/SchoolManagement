import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";
import { yupResolver } from "@hookform/resolvers/yup";
import { Label } from "@/components/ui/label";
import {
	Sheet,
	SheetClose,
	SheetContent,
	SheetDescription,
	SheetFooter,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import {
	Form,
	FormControl,
	FormDescription,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from "@/components/ui/form";

import { IStudent } from "@/types/student.type";
import { IStudentSchema, studentSchema } from "@/utils/schema/student.schema";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import DatePicker from "@/components/datepicker/DatePicker";

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
}
type IFormData = IStudentSchema;
export function Panel(props: IPanelProps) {
	const { type, selectedStudent, disable } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);

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

	console.log("errors", errors);

	const onSubmit = handleSubmit((data) => {
		// const value = getValues()
		// console.log(value)
		console.log("handle submit");
		console.log("data", data);
		setOpenSheet(false);
		reset(initValues);
		setCount(0);
	});

	const handleGetData = () => {
		if (count === 0) {
			fetch("https://jsonplaceholder.typicode.com/todos/1")
				.then((response) => response.json())
				.then((json) => {
					setValue("fullName", json.title);
					setCount((count) => count + 1);
				});
		}
	};

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
							Dân tộc
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
					<div>
						<Label htmlFor="academicYear" className="required">
							Niên khoá
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="academicYear"
							register={register}
							errorMessage={errors.academicYear?.message}
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
					<div>
						<Label htmlFor="status" className="required">
							Trạng thái học tập
						</Label>
						<CommonInput
							className="mt-[2px]"
							name="status"
							register={register}
							errorMessage={errors.status?.message}
						/>
					</div>
					<Button className="w-full" onClick={onSubmit}>
						Save changes
					</Button>
				</div>
			</SheetContent>
		</Sheet>
	);
}
