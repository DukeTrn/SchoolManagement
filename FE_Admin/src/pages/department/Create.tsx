import { createDepartment, updateDepartment } from "@/apis/department.api";
import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { IDepartment } from "@/types/department.type";
import {
	IDepartmentSchema,
	departmentSchema,
} from "@/utils/schema/department.schema";
import { yupResolver } from "@hookform/resolvers/yup";
import { useState, useReducer, useEffect } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	type: "edit" | "create";
	selected?: IDepartment | null;
	disable: boolean;
	refreshData: (value: string) => void;
}
const initValues = {
	departmentId: "",
	subjectName: "",
	description: "",
	notification: "",
};
export function Create(props: IPanelProps) {
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
	} = useForm<IDepartmentSchema>({
		defaultValues: initValues,
		resolver: yupResolver(departmentSchema),
	});

	useEffect(() => {
		setCount(0);
	}, [selected]);

	const handleGetData = () => {
		if (count === 0) {
			setValue("departmentId", selected?.departmentId as string);
			setValue("subjectName", selected?.subjectName as string);
			selected?.description &&
				setValue("description", selected?.description);
			selected?.notification &&
				setValue("notification", selected?.notification);
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
			departmentId: data?.departmentId,
			subjectName: data?.subjectName,
			description: data?.description ?? "",
			notification: data?.notification ?? "",
		};
		setLoading(true);
		if (type === "create") {
			createDepartment(body).then(() => {
				resetState();
				refreshData("Thêm tổ bộ môn thành công!");
			});
		} else {
			updateDepartment(body).then(() => {
				resetState();
				refreshData("Cập nhật tổ bộ môn thành công!");
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
						{type === "edit"
							? "Cập nhật tổ bộ môn"
							: "Thêm tổ bộ môn"}
					</SheetTitle>
				</SheetHeader>
				<div className="mt-5">
					<Label htmlFor="departmentId" className="required">
						Mã bộ môn
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="departmentId"
						register={register}
						errorMessage={errors.departmentId?.message}
					/>
				</div>
				<div>
					<Label htmlFor="subjectName" className="required">
						Tên bộ môn
					</Label>
					<CommonInput
						className="mt-[2px]"
						name="subjectName"
						register={register}
						errorMessage={errors.subjectName?.message}
					/>
				</div>
				<div>
					<Label htmlFor="description">Mô tả</Label>
					<CommonInput
						className="mt-[2px]"
						name="description"
						register={register}
					/>
				</div>
				<div>
					<Label htmlFor="notification">Thông báo</Label>
					<CommonInput
						className="mt-[2px]"
						name="notification"
						register={register}
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
