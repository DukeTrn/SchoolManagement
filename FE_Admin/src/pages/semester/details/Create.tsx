import { createSemesterDetail, getClassSemester } from "@/apis/semester.api";
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
	ISemesterDetailSchema,
	semesterDetailSchema,
} from "@/utils/schema/semester.schema";

import { yupResolver } from "@hookform/resolvers/yup";
import { useState } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	refreshData: (message: string) => void;
	id: string;
}
const initValues = {
	class: [],
};
export function Create({ refreshData, id }: IPanelProps) {
	const {
		reset,
		handleSubmit,
		formState: { errors },
		control,
	} = useForm<ISemesterDetailSchema>({
		defaultValues: initValues,
		resolver: yupResolver(semesterDetailSchema),
	});

	const [openSheet, setOpenSheet] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);
	const [classes, setClasses] = useState<
		{
			classId: string;
			className: string;
		}[]
	>([]);

	const handleGetData = () => {
		getClassSemester(id).then((res) => {
			setClasses(res?.data?.data);
		});
	};

	const onSubmit = handleSubmit((data) => {
		setLoading(true);
		createSemesterDetail({
			semesterId: id,
			classId: data?.class,
		}).then(() => {
			setLoading(false);
			reset(initValues);
			setOpenSheet(false);
			refreshData("Thêm lớp học vào học kỳ thành công");
		});
	});

	const list = classes?.map((item) => ({
		label: item?.className,
		value: item?.classId,
	}));

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<Button onClick={handleGetData}>Thêm</Button>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">
						Thêm lớp vào học kỳ
					</SheetTitle>
				</SheetHeader>
				<div className="mb-8 mt-5">
					<Label htmlFor="class" className="required ">
						Lớp học
					</Label>
					<FormField
						control={control}
						name="class"
						render={({ field }) => (
							<FormItem className="mt-[2px] flex flex-col">
								<MultiSelect
									options={list}
									onValueChange={field.onChange}
									// handleRetrieve={handleGetData}
									value={field.value}
									placeholder="Thêm lớp"
									variant="inverted"
									animation={2}
									maxCount={0}
									width={335}
									errorMessage={errors.class?.message!}
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
