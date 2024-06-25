import { getStudyClassDetail } from "@/apis/study.api";
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

export default function TeacherClassDetail() {
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
	const semester = state.semester;
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [searchValue, setSearchValue] = useState("");
	const subjectId = state.subjectId;
	const subjectName = state.subjectName;

	const searchQuery = useDebounce(searchValue, 1500);
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
			header: "Họ tên",
			cell: ({ row }) => {
				const fullName = row.original.fullName;
				return (
					<Link
						to={row.getValue("studentId")}
						state={{
							...row.original,
							subjectId,
							semester,
							subjectName,
						}}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{fullName}
					</Link>
				);
			},
		},
		{
			accessorKey: "grade",
			header: () => {
				return <div>Khối</div>;
			},
			cell: ({ row }) => <div>{row.getValue("grade")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "homeroomTeacherName",
			header: "Tình trạng học tập",
			cell: ({ row }) => <div>{row.getValue("homeroomTeacherName")}</div>,
		},
	];

	const initValue = {
		searchValue: searchQuery,
		pageSize: pageSize,
		pageNumber: pageNumber,
	};
	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, semester, searchQuery]);

	const handleGetData = () => {
		setLoading(true);
		getStudyClassDetail(
			initValue,
			state.grade,
			semester,
			state.classId
		).then((res) => {
			setStudent(res.data?.data.dataList);
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
