import { TableDetails } from "@/components/table/Table";
import Pagination from "@/components/pagination";
import { IClass } from "@/types/study.type";
import { ColumnDef } from "@tanstack/react-table";
import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useDebounce } from "@/hooks";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { ArrowLeft } from "lucide-react";
import { Button } from "@/components/ui/button";
import { getStudentClassDetail } from "@/apis/student.api";

export default function StudentClassDetail() {
	const location = useLocation();
	const navigation = useNavigate();
	const state = location?.state;
	const [student, setStudent] = useState<
		{
			fullName: string;
			className: string;
			grade: string;
			academicYear: string;
		}[]
	>([]);
	const [semester] = useState<string>(`${state.academicYear.split(" ")[0]}1`);
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [searchValue, setSearchValue] = useState("");
	const classId = state.classId;

	const searchQuery = useDebounce(searchValue, 1500);

	const renderStatus = (value: number) => {
		switch (value) {
			case 1:
				return "Đang học";
			case 2:
				return "Đình chỉ";
			case 3:
				return "Nghỉ học";
		}
	};

	const columns: ColumnDef<IClass>[] = [
		{
			accessorKey: "stt",
			header: () => {
				return <div>STT</div>;
			},
			cell: ({ row }) => <div>{row.index + 1}</div>,
			minSize: 200,
		},

		{
			accessorKey: "studentId",
			header: "MSHS",
			cell: ({ row }) => {
				const studentId = row.original.studentId;
				return (
					<Link
						to={row.getValue("studentId")}
						state={row.original}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{studentId}
					</Link>
				);
			},
		},
		{
			accessorKey: "fullName",
			header: () => {
				return <div>Họ Tên</div>;
			},
			cell: ({ row }) => <div>{row.getValue("fullName")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "status",
			header: "Tình trạng học tập",
			cell: ({ row }) => (
				<div>{renderStatus(row.getValue("status"))}</div>
			),
		},
	];

	const initValue = {
		searchValue: searchQuery,
		pageSize: pageSize,
		pageNumber: pageNumber,
		status: [1, 2],
	};
	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, semester, searchQuery]);

	const handleGetData = () => {
		setLoading(true);
		getStudentClassDetail(initValue, state.classId).then((res) => {
			setStudent(res.data?.dataList);
			setLoading(false);
		});
	};
	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">{`DANH SÁCH HỌC SINH LỚP HỌC ${state.className}`}</div>
			</div>
			<div className="relative mb-5 max-w-[300px]">
				<Search className="absolute left-2 top-2.5 h-4 w-4 " />
				<Input
					width={300}
					placeholder="Tìm kiếm"
					className="pl-8"
					onChange={(e) => setSearchValue(e.target?.value)}
				/>
			</div>
			<div className="mb-5">
				<Button
					onClick={() => {
						navigation(`/student-class/timetable/${semester}`, {
							state: {
								semester: semester,
								classId: classId,
							},
						});
					}}
				>
					Thời khóa biểu
				</Button>
			</div>
			<div>
				<TableDetails
					pageSize={10}
					data={student ?? []}
					columns={columns}
					//onChange={handleChange}
					loading={loading}
				/>
				<Pagination
					pageSize={pageSize}
					onChangePage={(value) => setPageNumber(Number(value))}
					onChangeRow={(value) => setPageSize(Number(value))}
					totalPageCount={totalPage}
				/>
			</div>
		</>
	);
}
