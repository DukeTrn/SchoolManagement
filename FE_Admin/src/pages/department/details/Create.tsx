import {
	addTeacherDepartment,
	getTeacherDepartment,
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
import { useState } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	refreshData: (message: string) => void;
	departmentId: string;
}
const initValues = {
	teacher: [],
};
export function Create({ refreshData, departmentId }: IPanelProps) {
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

	console.log("errors", errors);

	const handleGetData = () => {
		getTeacherDepartment().then((res) => {
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

	const list = teachers?.map((item) => ({
		label: item?.fullName,
		value: item?.teacherId,
	}));

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button onClick={handleGetData}>Thêm</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">
						Thêm giáo viên
					</SheetTitle>
				</SheetHeader>
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
									placeholder="Tình trạng học tập"
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
				<Button className="w-full" onClick={onSubmit} loading={loading}>
					Lưu
				</Button>
			</SheetContent>
		</Sheet>
	);
}
