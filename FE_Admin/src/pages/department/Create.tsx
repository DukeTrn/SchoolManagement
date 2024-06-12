import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
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
import { IDepartment } from "@/types/department.type";
import { useState, useReducer } from "react";

interface IPanelState {
	id: string;
	name: string;
	description: string;
	notification: string;
}
interface IPanelProps {
	type: "edit" | "create";
	selected?: IDepartment | null;
	disable: boolean;
}

export function Create(props: IPanelProps) {
	const { type, selected, disable } = props;
	const [openSheet, setOpenSheet] = useState(false);

	const [formValue, handleForm] = useReducer(
		(prev: IPanelState, next: Partial<IPanelState>) => {
			return { ...prev, ...next };
		},
		{ id: "", name: "", description: "", notification: "" }
	);

	const handleChange =
		(name: "id" | "name" | "description" | "notification") =>
		(event: React.ChangeEvent<HTMLInputElement>) => {
			handleForm({
				[name]: event.target?.value,
			});
		};

	const handleGetData = () => {};

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
					<Label htmlFor="departmentId">Mã bộ môn</Label>
					<Input
						name="departmentId"
						className="mt-[2px]"
						value={formValue?.id}
						onChange={handleChange("id")}
					/>
				</div>
				<div className="mt-5">
					<Label htmlFor="subjectName">Tên bộ môn</Label>
					<Input
						name="subjectName"
						className="mt-[2px]"
						value={formValue?.name}
						onChange={handleChange("name")}
					/>
				</div>
				<div className="mt-5">
					<Label htmlFor="description">Mô tả</Label>
					<Input
						name="description"
						className="mt-[2px]"
						value={formValue?.description}
						onChange={handleChange("description")}
					/>
				</div>
				<div className="mt-5">
					<Label htmlFor="notification">Thông báo</Label>
					<Input
						name="notification"
						className="mt-[2px]"
						value={formValue?.notification}
						onChange={handleChange("notification")}
					/>
				</div>
				<div className="mt-10">
					<Button className="w-full">Save changes</Button>
				</div>
			</SheetContent>
		</Sheet>
	);
}
