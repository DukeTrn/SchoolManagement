import { addStudentToClass, getFreeStudent } from "@/apis/classroom.api";
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
import { IClassroom } from "@/types/classroom.type";
import {
	IClassroomStudentDetailSchema,
	classroomStudentDetailSchema,
} from "@/utils/schema/classroom.schema";
import { yupResolver } from "@hookform/resolvers/yup";
import { useState } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	refreshData: (message: string) => void;
	id: string;
	state: IClassroom;
}
const initValues = {
	studentsValue: [],
};
export function Create({ refreshData, id, state }: IPanelProps) {
	const {
		reset,
		handleSubmit,
		formState: { errors },
		control,
	} = useForm<IClassroomStudentDetailSchema>({
		defaultValues: initValues,
		resolver: yupResolver(classroomStudentDetailSchema),
	});

	const [openSheet, setOpenSheet] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);
	const [students, setStudents] = useState<
		{
			studentId: string;
			fullName: string;
		}[]
	>([]);

	const handleGetData = () => {
		getFreeStudent({
			academicYear: state?.academicYear,
			grade: Number(state?.grade),
		}).then((res) => {
			setStudents(res?.data?.data);
		});
	};

	const onSubmit = handleSubmit((data) => {
		setLoading(true);
		addStudentToClass(
			data?.studentsValue?.map((i) => ({
				classId: id,
				studentId: i,
			})) as {
				classId: string;
				studentId: string;
			}[]
		).then(() => {
			setLoading(false);
			reset(initValues);
			setOpenSheet(false);
			refreshData("Thêm học sinh vào lớp thành công");
		});
	});

	const list = students?.map((item) => ({
		label: item?.fullName,
		value: item?.studentId,
	}));

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button onClick={handleGetData}>Thêm</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">Thêm học sinh</SheetTitle>
				</SheetHeader>
				<div className="mb-8 mt-5">
					<Label htmlFor="studentsValue" className="required ">
						Học sinh:
					</Label>
					<FormField
						control={control}
						name="studentsValue"
						render={({ field }) => (
							<FormItem className="mt-[2px] flex flex-col">
								<MultiSelect
									options={list}
									onValueChange={field.onChange}
									// handleRetrieve={handleGetData}
									value={field.value}
									placeholder="Thêm học sinh"
									variant="inverted"
									animation={2}
									maxCount={0}
									width={335}
									errorMessage={errors.studentsValue?.message}
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
