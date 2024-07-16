import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { ITimetable } from "@/types/timetable.type";
import { ColumnDef } from "@tanstack/react-table";
import { ArrowLeft } from "lucide-react";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { toast } from "@/components/ui/use-toast";
import { TimetableDetail } from "@/pages/classroom/details/TimeTableDetail";
import { getStudentTimetableList } from "@/apis/timetable.api";

const StudentTimetable = () => {
	const location = useLocation();
	const state = location?.state;
	const navigation = useNavigate();

	const [timetableList, setTimetableList] = useState<ITimetable[]>([]);
	const [loading, setLoading] = useState<boolean>(false);

	const getPeriod = (id: number) =>
		timetableList
			?.filter((item) => item.period == id)
			?.map((i) => ({
				dayOfWeek: i?.dayOfWeek,
				subjectName: i?.subjectName,
				timetableId: i?.timetableId,
				assignmentId: i?.assignmentId,
				className: i?.className,
			}));
	const period1 = getPeriod(1);
	const period2 = getPeriod(2);
	const period3 = getPeriod(3);
	const period4 = getPeriod(4);
	const period5 = getPeriod(5);

	const items = [
		{
			title: "Tiết 1",
			day2: period1?.find((i) => i.dayOfWeek === 2)?.subjectName,
			day3: period1?.find((i) => i.dayOfWeek === 3)?.subjectName,
			day4: period1?.find((i) => i.dayOfWeek === 4)?.subjectName,
			day5: period1?.find((i) => i.dayOfWeek === 5)?.subjectName,
			day6: period1?.find((i) => i.dayOfWeek === 6)?.subjectName,
			day7: period1?.find((i) => i.dayOfWeek === 7)?.subjectName,
			timetableId2: period1?.find((i) => i.dayOfWeek === 2)?.timetableId,
			timetableId3: period1?.find((i) => i.dayOfWeek === 3)?.timetableId,
			timetableId4: period1?.find((i) => i.dayOfWeek === 4)?.timetableId,
			timetableId5: period1?.find((i) => i.dayOfWeek === 5)?.timetableId,
			timetableId6: period1?.find((i) => i.dayOfWeek === 6)?.timetableId,
			timetableId7: period1?.find((i) => i.dayOfWeek === 7)?.timetableId,
		},
		{
			title: "Tiết 2",
			day2: period2?.find((i) => i.dayOfWeek === 2)?.subjectName,
			day3: period2?.find((i) => i.dayOfWeek === 3)?.subjectName,
			day4: period2?.find((i) => i.dayOfWeek === 4)?.subjectName,
			day5: period2?.find((i) => i.dayOfWeek === 5)?.subjectName,
			day6: period2?.find((i) => i.dayOfWeek === 6)?.subjectName,
			day7: period2?.find((i) => i.dayOfWeek === 7)?.subjectName,
			timetableId2: period2?.find((i) => i.dayOfWeek === 2)?.timetableId,
			timetableId3: period2?.find((i) => i.dayOfWeek === 3)?.timetableId,
			timetableId4: period2?.find((i) => i.dayOfWeek === 4)?.timetableId,
			timetableId5: period2?.find((i) => i.dayOfWeek === 5)?.timetableId,
			timetableId6: period2?.find((i) => i.dayOfWeek === 6)?.timetableId,
			timetableId7: period2?.find((i) => i.dayOfWeek === 7)?.timetableId,
		},
		{
			title: "Tiết 3",
			day2: period3?.find((i) => i.dayOfWeek === 2)?.subjectName,
			day3: period3?.find((i) => i.dayOfWeek === 3)?.subjectName,
			day4: period3?.find((i) => i.dayOfWeek === 4)?.subjectName,
			day5: period3?.find((i) => i.dayOfWeek === 5)?.subjectName,
			day6: period3?.find((i) => i.dayOfWeek === 6)?.subjectName,
			day7: period3?.find((i) => i.dayOfWeek === 7)?.subjectName,
			timetableId2: period3?.find((i) => i.dayOfWeek === 2)?.timetableId,
			timetableId3: period3?.find((i) => i.dayOfWeek === 3)?.timetableId,
			timetableId4: period3?.find((i) => i.dayOfWeek === 4)?.timetableId,
			timetableId5: period3?.find((i) => i.dayOfWeek === 5)?.timetableId,
			timetableId6: period3?.find((i) => i.dayOfWeek === 6)?.timetableId,
			timetableId7: period3?.find((i) => i.dayOfWeek === 7)?.timetableId,
		},
		{
			title: "Tiết 4",
			day2: period4?.find((i) => i.dayOfWeek === 2)?.subjectName,
			day3: period4?.find((i) => i.dayOfWeek === 3)?.subjectName,
			day4: period4?.find((i) => i.dayOfWeek === 4)?.subjectName,
			day5: period4?.find((i) => i.dayOfWeek === 5)?.subjectName,
			day6: period4?.find((i) => i.dayOfWeek === 6)?.subjectName,
			day7: period4?.find((i) => i.dayOfWeek === 7)?.subjectName,
			timetableId2: period4?.find((i) => i.dayOfWeek === 2)?.timetableId,
			timetableId3: period4?.find((i) => i.dayOfWeek === 3)?.timetableId,
			timetableId4: period4?.find((i) => i.dayOfWeek === 4)?.timetableId,
			timetableId5: period4?.find((i) => i.dayOfWeek === 5)?.timetableId,
			timetableId6: period4?.find((i) => i.dayOfWeek === 6)?.timetableId,
			timetableId7: period4?.find((i) => i.dayOfWeek === 7)?.timetableId,
		},
		{
			title: "Tiết 5",
			day2: period5?.find((i) => i.dayOfWeek === 2)?.subjectName,
			day3: period5?.find((i) => i.dayOfWeek === 3)?.subjectName,
			day4: period5?.find((i) => i.dayOfWeek === 4)?.subjectName,
			day5: period5?.find((i) => i.dayOfWeek === 5)?.subjectName,
			day6: period5?.find((i) => i.dayOfWeek === 6)?.subjectName,
			day7: period5?.find((i) => i.dayOfWeek === 7)?.subjectName,
			timetableId2: period5?.find((i) => i.dayOfWeek === 2)?.timetableId,
			timetableId3: period5?.find((i) => i.dayOfWeek === 3)?.timetableId,
			timetableId4: period5?.find((i) => i.dayOfWeek === 4)?.timetableId,
			timetableId5: period5?.find((i) => i.dayOfWeek === 5)?.timetableId,
			timetableId6: period5?.find((i) => i.dayOfWeek === 6)?.timetableId,
			timetableId7: period5?.find((i) => i.dayOfWeek === 7)?.timetableId,
		},
	];

	const columns: ColumnDef<any>[] = [
		{
			accessorKey: "title",
			header: "Sáng",
			cell: ({ row }) => (
				<div className="font-medium">{row.getValue("title")}</div>
			),
			minSize: 80,
		},
		{
			accessorKey: "day2",
			header: () => {
				return <div>Thứ 2</div>;
			},
			cell: ({ row }) => {
				const timetableId2 = row.original.timetableId2;
				return (
					<TimetableDetail
						timetableName={row.getValue("day2")}
						timetableId={timetableId2}
						refreshData={refreshData}
					/>
				);
			},
		},
		{
			accessorKey: "day3",
			header: () => {
				return <div>Thứ 3</div>;
			},
			cell: ({ row }) => {
				const timetableId = row.original.timetableId3;
				return (
					<TimetableDetail
						timetableName={row.getValue("day3")}
						timetableId={timetableId}
						refreshData={refreshData}
					/>
				);
			},
		},
		{
			accessorKey: "day4",
			header: () => {
				return <div>Thứ 4</div>;
			},
			cell: ({ row }) => {
				const timetableId = row.original.timetableId4;
				return (
					<TimetableDetail
						timetableName={row.getValue("day4")}
						timetableId={timetableId}
						refreshData={refreshData}
					/>
				);
			},
		},
		{
			accessorKey: "day5",
			header: () => {
				return <div>Thứ 5</div>;
			},
			cell: ({ row }) => {
				const timetableId = row.original.timetableId5;
				return (
					<TimetableDetail
						timetableName={row.getValue("day5")}
						timetableId={timetableId}
						refreshData={refreshData}
					/>
				);
			},
		},
		{
			accessorKey: "day6",
			header: () => {
				return <div>Thứ 6</div>;
			},
			cell: ({ row }) => {
				const timetableId = row.original.timetableId6;
				return (
					<TimetableDetail
						refreshData={refreshData}
						timetableName={row.getValue("day6")}
						timetableId={timetableId}
					/>
				);
			},
		},
		{
			accessorKey: "day7",
			header: () => {
				return <div>Thứ 7</div>;
			},
			cell: ({ row }) => {
				const timetableId = row.original.timetableId7;
				return (
					<TimetableDetail
						refreshData={refreshData}
						timetableName={row.getValue("day7")}
						timetableId={timetableId}
					/>
				);
			},
		},
	];

	const handleGetData = () => {
		setLoading(true);
		getStudentTimetableList(state?.classId, state?.semester).then((res) => {
			setLoading(false);
			setTimetableList(res?.data);
		});
	};

	const refreshData = (message: string, varient?: boolean) => {
		handleGetData();
		toast({
			title: "Thông báo:",
			description: message,
			className: "border-2 border-green-500 p-4",
			variant: varient ? "destructive" : "default",
		});
	};

	useEffect(() => {
		handleGetData();
	}, []);
	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium uppercase">{`Lịch lớp giảng dạy học kỳ ${state?.semester}`}</div>
			</div>

			<div>
				<div>
					<TableDetails
						pageSize={10}
						data={timetableList?.length > 0 ? items : []}
						columns={columns}
						loading={loading}
					/>
				</div>
			</div>

			<div className="mt-10">
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					Chú Thích
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• Tiết 1: 7h - 7h45 (Sáng), 13h - 13h45 (Chiều)
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• Tiết 2: 8h-8h45 (Sáng), 14h - 14h45 (Chiều)
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• Tiết 3: 9h - 9h45 (Sáng), 15h - 15h45 (Chiều)
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• Tiết 4: 10h - 10h45 (Sáng), 16h - 16h45 (Chiều)
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• Tiết 5: 11h - 11h45, (Sáng), 17h - 17h45 (Chiều)
				</div>
			</div>
		</>
	);
};

export default StudentTimetable;
