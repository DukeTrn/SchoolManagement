import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import Pagination from "@/components/pagination";
import { ColumnDef } from "@tanstack/react-table";
import { IClass } from "@/types/study.type";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import { getTeacherSemesterScore } from "@/apis/teacher.info.api";

const TeacherHomeroomSemester = () => {
	const location = useLocation();
	const navigation = useNavigate();
	const state = location?.state;
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [student, setStudent] = useState<any>([]);
	// const [point, setPoint] = useState<any>();
	console.log(state);
	const columns: ColumnDef<IClass>[] = [
		{
			accessorKey: "subjectName",
			header: () => {
				return <div>Môn học</div>;
			},
			cell: ({ row }) => <div>{row.getValue("subjectName")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "average",
			header: () => {
				return <div>ĐTBmhk</div>;
			},
			cell: ({ row }) => <div>{row.getValue("average")}</div>,
		},
	];

	const conductToResult = (value: number) => {
		if (value === 1) return "Tốt";
		else if (value === 2) return "Khá";
		else if (value === 3) return "Trung Bình";
		else return "Kém";
	};

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize]);

	const handleGetData = () => {
		setLoading(true);
		getTeacherSemesterScore(
			state.grade,
			state.semester,
			state.classDetailId
		).then((res) => {
			setStudent(res.data?.data);
			setLoading(false);
		});
	};

	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">{`KẾT QUẢ HỌC TẬP HỌC SINH ${state?.fullName.toUpperCase()} `}</div>
			</div>
			<div className="flex">
				<div className=" [&>[role=checkbox]]:translate-y-[2px]text-sm mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0">
					{`Học lực: ${student?.academicPerform}`}
				</div>
				<div className=" [&>[role=checkbox]]:translate-y-[2px]text-sm mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0">
					{`Hạnh kiểm: ${conductToResult(student?.conductName)}`}
				</div>
				<div className=" [&>[role=checkbox]]:translate-y-[2px]text-sm mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0">
					{`Danh hiệu: ${student?.title}`}
				</div>
			</div>
			<div className="mb-5 mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
				{`Điểm TB: ${student?.totalAverage}`}
			</div>

			<div>
				<TableDetails
					pageSize={pageSize}
					data={student?.subjects ?? []}
					columns={columns}
					//onChange={handleChange}
					loading={loading}
				/>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					Chú Thích
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐTBmhk: Điểm Trung Bình môn học kỳ
				</div>
				<Pagination
					pageSize={pageSize}
					onChangePage={(value) => setPageNumber(Number(value))}
					onChangeRow={(value) => setPageSize(Number(value))}
					totalPageCount={totalPage}
				/>
			</div>
		</>
	);
};

export default TeacherHomeroomSemester;
