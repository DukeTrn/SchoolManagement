import { getClassFilter, updateStudentClass } from "@/apis/classroom.api";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
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
import { IClassroom } from "@/types/classroom.type";
import {
	IClassroomFreeSchema,
	classroomFreeSchema,
} from "@/utils/schema/classroom.schema";

import { yupResolver } from "@hookform/resolvers/yup";
import { useState } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	refreshData: (message: string) => void;
	id: string;
	state: IClassroom;
	disable: boolean;
}
const initValues = {
	classIds: "",
};
export function Edit({ refreshData, id, state, disable }: IPanelProps) {
	const {
		reset,
		handleSubmit,
		formState: { errors },
		control,
	} = useForm<IClassroomFreeSchema>({
		defaultValues: initValues,
		resolver: yupResolver(classroomFreeSchema),
	});

	const [openSheet, setOpenSheet] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);
	const [classroom, setClassroom] = useState<
		{ classId: string; className: string }[]
	>([]);

	const handleGetData = () => {
		getClassFilter(state?.academicYear, Number(state?.grade)).then(
			(res) => {
				setClassroom(res?.data?.data);
			}
		);
	};

	const onSubmit = handleSubmit((data) => {
		setLoading(true);
		updateStudentClass(state?.classId, data?.classIds).then(() => {
			setLoading(false);
			reset(initValues);
			setOpenSheet(false);
			refreshData("Chuyển lớp thành công");
		});
	});

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button onClick={handleGetData} disabled={disable}>
					Chuyển lớp
				</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">Chuyển lớp</SheetTitle>
				</SheetHeader>
				<div className="mb-8 mt-5">
					<Label htmlFor="classIds" className="required">
						Lớp học:
					</Label>
					<div className="mt-1">
						<FormField
							control={control}
							name="classIds"
							render={({ field }) => (
								<Select
									value={field.value}
									onValueChange={field.onChange}
								>
									<SelectTrigger className="w-full">
										<SelectValue placeholder="Chọn lớp học" />
									</SelectTrigger>
									<SelectContent className="w-full">
										<SelectGroup>
											{classroom?.map((i) => (
												<SelectItem
													value={i.classId}
													key={i.classId}
												>
													{i.className}
												</SelectItem>
											))}
										</SelectGroup>
									</SelectContent>
								</Select>
							)}
						/>
					</div>

					<div className="mt-1 min-h-[1.25rem] text-sm text-red-600">
						{errors?.classIds?.message}
					</div>
				</div>
				<Button className="w-full" onClick={onSubmit} loading={loading}>
					Lưu
				</Button>
			</SheetContent>
		</Sheet>
	);
}
