import {
	Sheet,
	SheetContent,
	SheetFooter,
	SheetHeader,
	SheetTitle,
	SheetTrigger,
} from "@/components/ui/sheet";
import { useState } from "react";
import { getTimetableDetail, removeTimetable } from "@/apis/timetable.api";
import { Button } from "@/components/ui/button";
import { ITimetable } from "@/types/timetable.type";
import { EditTimeTable } from "./EditTimetable";

interface IProps {
	timetableId: string;
	timetableName: string;
	refreshData: (message: string) => void;
}

export function TimetableDetail(props: IProps) {
	const { timetableId, timetableName, refreshData } = props;
	const [timetable, setTimetable] = useState<ITimetable>();
	const [openSheet, setOpenSheet] = useState(false);
	const [openEditSheet, setOpenEditSheet] = useState(false);

	const getStudentDetails = () => {
		getTimetableDetail(timetableId).then((res) => {
			setTimetable(res?.data?.data);
		});
	};

	const handleDelete = () => {
		removeTimetable(timetableId).then((res) => {
			refreshData(res?.data?.message);
		});
	};

	return (
		<Sheet open={openSheet} onOpenChange={setOpenSheet}>
			<SheetTrigger asChild>
				<div
					className="cursor-pointer font-medium text-blue-600 underline"
					onClick={getStudentDetails}
				>
					{timetableName}
				</div>
			</SheetTrigger>
			<SheetContent>
				<SheetHeader>
					<SheetTitle className="uppercase">
						Chi tiết thời khoá biểu
					</SheetTitle>
				</SheetHeader>
				<div className="flex h-full flex-col pb-5">
					<div className="mt-6 flex-1">
						<div>
							<div className="mb-3 flex">
								<span className="mr-4 font-medium">
									Lớp học:
								</span>
								<span>{timetable?.className}</span>
							</div>
							<div className="mb-3 flex">
								<span className="mr-4 font-medium">
									Môn học:
								</span>
								<span>{timetable?.subjectName}</span>
							</div>
							<div className="mb-3 flex">
								<span className="mr-4 font-medium">
									Giáo viên:
								</span>
								<span>{timetable?.teacherName}</span>
							</div>
							<div className="mb-3 flex">
								<span className="mr-4 font-medium">Thứ:</span>
								<span>{timetable?.dayOfWeek}</span>
							</div>
							<div className="mb-3 flex">
								<span className="mr-4 font-medium">Tiết:</span>
								<span>{timetable?.period}</span>
							</div>
						</div>
					</div>
					<SheetFooter>
						<div className="flex w-full gap-4">
							<div className="w-[50%]">
								<EditTimeTable
									refreshData={refreshData}
									openEditSheet={openEditSheet}
									setOpenSheet={setOpenSheet}
									setOpenEditSheet={setOpenEditSheet}
									timetable={timetable!}
								/>
							</div>
							<Button className="w-[50%]" onClick={handleDelete}>
								Xoá
							</Button>
						</div>
					</SheetFooter>
				</div>
			</SheetContent>
		</Sheet>
	);
}
