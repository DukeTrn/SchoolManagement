import { createDepartment, updateDepartment } from "@/apis/department.api";
import { createTeacher, updateTeacher } from "@/apis/teacher.info.api";
import CommonInput from "@/components/input/CommonInput";
import { Button } from "@/components/ui/button";

import { Label } from "@/components/ui/label";
import {
	Sheet,
	SheetContent,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { ITeacherCreateScore } from "@/types/teacher.info.type";
import { teacherInfoSchema } from "@/utils/schema/teacher.info.schema";
import { yupResolver } from "@hookform/resolvers/yup";
import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";

interface IPanelProps {
	type: "edit" | "create";
	selected?: ITeacherCreateScore | null;
	disable: boolean;
	refreshData: (value: string) => void;
	items: any;
	student: any;
}

export function Create(props: IPanelProps) {
	const { type, selected, disable, refreshData } = props;
	const [openSheet, setOpenSheet] = useState(false);
	const [count, setCount] = useState(0);
	const [loading, setLoading] = useState(false);
	const itemWeight1 = props.student.weight1
		? props.student.weight1?.map((item: any) => item.score)
		: [];
	const itemWeight2 = props.student.weight2
		? props.student.weight2?.map((item: any) => item.score)
		: [];
	const itemWeight3 = props.student.weight3
		? props.student.weight3?.map((item: any) => item.score)
		: [];

	const item1 = props.student.weight1
		? props.student.weight1?.map((item: any) => item)
		: [];
	const item2 = props.student.weight2
		? props.student.weight2?.map((item: any) => item)
		: [];
	const item3 = props.student.weight3
		? props.student.weight3?.map((item: any) => item)
		: [];

	const initValues = [
		{
			score: 0,
			weight: 0,
			feedback: "",
			semesterId: props.items.semester,
			subjectId: props.items.subjectId,
			classDetailId: props.items.classDetailId,
		},
		{
			score: 0,
			weight: 0,
			feedback: "",
			semesterId: props.items.semester,
			subjectId: props.items.subjectId,
			classDetailId: props.items.classDetailId,
		},
		{
			score: 0,
			weight: 0,
			feedback: "",
			semesterId: props.items.semester,
			subjectId: props.items.subjectId,
			classDetailId: props.items.classDetailId,
		},
		{
			score: 0,
			weight: 0,
			feedback: "",
			semesterId: props.items.semester,
			subjectId: props.items.subjectId,
			classDetailId: props.items.classDetailId,
		},
		{
			score: 0,
			weight: 0,
			feedback: "",
			semesterId: props.items.semester,
			subjectId: props.items.subjectId,
			classDetailId: props.items.classDetailId,
		},
		{
			score: 0,
			weight: 0,
			feedback: "",
			semesterId: props.items.semester,
			subjectId: props.items.subjectId,
			classDetailId: props.items.classDetailId,
		},
		{
			score: 0,
			weight: 0,
			feedback: "",
			semesterId: props.items.semester,
			subjectId: props.items.subjectId,
			classDetailId: props.items.classDetailId,
		},
	];

	const {
		register,
		handleSubmit,
		formState: { errors },
		setValue,
		reset,
	} = useForm<any>({
		defaultValues: initValues,
		resolver: yupResolver(teacherInfoSchema),
	});

	useEffect(() => {
		setCount(0);
	}, [selected]);

	const handleGetData = () => {
		if (count === 0 && type === "edit") {
			setValue("ĐĐGtx1", itemWeight1[0]);
			setValue("ĐĐGtx2", itemWeight1[1]);
			setValue("ĐĐGtx3", itemWeight1[2]);
			setValue("ĐĐGtx4", itemWeight1[3]);
			setValue("ĐĐGgk1", itemWeight2[0]);
			setValue("ĐĐGgk2", itemWeight2[1]);
			setValue("ĐĐGck", itemWeight3[0]);
			setCount((count) => count + 1);
		}
	};

	const resetState = () => {
		setLoading(false);
		setCount(0);
		setOpenSheet(false);
	};

	const onSubmit = handleSubmit((data) => {
		const body = [];
		if (type === "create") {
			for (let key in data) {
				if (key.includes("tx") && data[key])
					body.push({
						score: data[key],
						weight: 1,
						feedback: "",
						semesterId: props.items.semester,
						subjectId: props.items.subjectId,
						classDetailId: props.items.classDetailId,
					});
				else if (key.includes("gk") && data[key])
					body.push({
						score: data[key],
						weight: 2,
						feedback: "",
						semesterId: props.items.semester,
						subjectId: props.items.subjectId,
						classDetailId: props.items.classDetailId,
					});
				else if (key.includes("ck") && data[key])
					body.push({
						score: data[key],
						weight: 3,
						feedback: "",
						semesterId: props.items.semester,
						subjectId: props.items.subjectId,
						classDetailId: props.items.classDetailId,
					});
			}
		} else if (type === "edit") {
			const itemEdit1 = [];
			const itemEdit2 = [];
			const itemEdit3 = [];
			for (let key in data) {
				if (key.includes("tx")) {
					itemEdit1.push(data[key]);
				} else if (key.includes("gk")) {
					itemEdit2.push(data[key]);
				} else if (key.includes("ck")) {
					itemEdit3.push(data[key]);
				}
			}
			if (itemEdit1.length) {
				for (let i = 0; i < itemEdit1.length; i++) {
					if (
						Number(itemEdit1[i]) !== itemWeight1[i] &&
						item1[i]?.assessmentId
					) {
						body.push({
							assessmentId: item1[i].assessmentId,
							score: itemEdit1[i],
							feedback: item1[i].feedback,
						});
					}
				}
			}
			if (itemEdit2.length) {
				for (let i = 0; i < itemEdit2.length; i++) {
					if (
						Number(itemEdit2[i]) !== itemWeight2[i] &&
						item2[i]?.assessmentId
					) {
						body.push({
							assessmentId: item2[i].assessmentId,
							score: itemEdit2[i],
							feedback: item2[i].feedback,
						});
					}
				}
			}

			if (
				Number(itemEdit3[0]) !== itemWeight3[0] &&
				item3[0]?.assessmentId
			) {
				body.push({
					assessmentId: item3[0].assessmentId,
					score: itemEdit3[0],
					feedback: item3[0].feedback,
				});
			}
		}
		setLoading(true);
		if (type === "create") {
			createTeacher(body as any).then(() => {
				resetState();
				refreshData("Thêm điểm thành công!");
			});
		} else {
			updateTeacher(body as any).then(() => {
				resetState();
				refreshData("Cập nhật điểm thành công!");
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
					<SheetTitle className="mb-5 uppercase">
						{type === "edit" ? "Cập nhật điểm" : "Thêm điểm"}
					</SheetTitle>
				</SheetHeader>
				{type === "create" && (
					<div>
						<div>
							<div className="font-medium">Điểm hệ số 1</div>
							<div>
								<div className="">
									<Label
										htmlFor="description"
										className="justify-center text-center align-middle"
									>
										ĐĐGtx1
									</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx1"
										register={register}
										disabled={itemWeight1[0]}
										value={itemWeight1[0]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGtx2</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx2"
										register={register}
										disabled={itemWeight1[1]}
										value={itemWeight1[1]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGtx3</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx3"
										register={register}
										disabled={itemWeight1[2]}
										value={itemWeight1[2]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGtx4</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx4"
										register={register}
										disabled={itemWeight1[3]}
										value={itemWeight1[3]}
									/>
								</div>
							</div>
						</div>
						<div>
							<div className="font-medium">Điểm hệ số 2</div>
							<div>
								<div className="">
									<Label htmlFor="description">ĐĐGgk1</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGgk1"
										register={register}
										disabled={itemWeight2[0]}
										value={itemWeight2[0]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGgk2</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGgk2"
										register={register}
										disabled={itemWeight2[1]}
										value={itemWeight2[1]}
									/>
								</div>
							</div>
						</div>

						<div className="gap-5">
							<div className="font-medium">Điểm hệ số 3</div>
							<div className="">
								<Label htmlFor="description">ĐĐGck</Label>
								<CommonInput
									className="mt-[2px]"
									name="ĐĐGck"
									register={register}
									disabled={itemWeight3[0]}
									value={itemWeight3[0]}
								/>
							</div>
						</div>
					</div>
				)}

				{type === "edit" && (
					<div>
						<div>
							<div className="font-medium">Điểm hệ số 1</div>
							<div>
								<div className="">
									<Label
										htmlFor="description"
										className="justify-center text-center align-middle"
									>
										ĐĐGtx1
									</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx1"
										register={register}
										disabled={!itemWeight1[0]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGtx2</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx2"
										register={register}
										disabled={!itemWeight1[1]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGtx3</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx3"
										register={register}
										disabled={!itemWeight1[2]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGtx4</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGtx4"
										register={register}
										disabled={!itemWeight1[3]}
									/>
								</div>
							</div>
						</div>
						<div>
							<div className="font-medium">Điểm hệ số 2</div>
							<div>
								<div className="">
									<Label htmlFor="description">ĐĐGgk1</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGgk1"
										register={register}
										disabled={!itemWeight2[0]}
									/>
								</div>
								<div className="">
									<Label htmlFor="description">ĐĐGgk2</Label>
									<CommonInput
										className="mt-[2px]"
										name="ĐĐGgk2"
										register={register}
										disabled={!itemWeight2[1]}
									/>
								</div>
							</div>
						</div>

						<div className="gap-5">
							<div className="font-medium">Điểm hệ số 3</div>
							<div className="">
								<Label htmlFor="description">ĐĐGck</Label>
								<CommonInput
									className="mt-[2px]"
									name="ĐĐGck"
									register={register}
									disabled={!itemWeight3[0]}
								/>
							</div>
						</div>
					</div>
				)}

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
