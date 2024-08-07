import { useEffect, useState } from "react";
import { getStudyClassDetail } from "@/apis/study.api";
import { Link, useLocation, useNavigate } from "react-router-dom";
import Pagination from "@/components/pagination";
import { ColumnDef } from "@tanstack/react-table";
import { IClass } from "@/types/study.type";
import { CustomTable } from "@/components/customTable/CustomTable";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import { getAllFilterSemester } from "@/apis/semester.api";
import { IFilterSemesters } from "@/types/semester.type";

const StudyClassDetail = () => {
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
	const [semester, setSemester] = useState<string>(
		`${state.year.split(" ")[0]}1`
	);
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [semesters, setSemesters] = useState<IFilterSemesters[]>([]);

	const columns: ColumnDef<IClass>[] = [
		{
			accessorKey: "classDetailId",
			header: "Họ tên",
			cell: ({ row }) => {
				const fullName = row.original.fullName;
				return (
					<Link
						to={row.getValue("classDetailId")}
						state={{ ...row.original, semester }}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{fullName}
					</Link>
				);
			},
		},
		{
			accessorKey: "className",
			header: () => {
				return <div>Lớp học</div>;
			},
			cell: ({ row }) => <div>{row.getValue("className")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "grade",
			header: "Khối",
			cell: ({ row }) => <div>{row.getValue("grade")}</div>,
		},
		{
			accessorKey: "academicYear",
			header: "Niên khóa",
			cell: ({ row }) => <div>{row.getValue("academicYear")}</div>,
		},
	];

	const initValue = {
		searchValue: "",
		pageSize: pageSize,
		pageNumber: pageNumber,
	};
	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, semester]);

	const handleGetData = () => {
		setLoading(true);
		getAllFilterSemester().then((res) => {
			setSemesters(res?.data?.data);
		});
		getStudyClassDetail(
			initValue,
			Number(state.selectedField),
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
				<div className="mb-4 text-2xl font-medium">{`QUẢN LÝ HỌC TẬP LỚP ${state.className} NIÊN KHÓA ${state.year}`}</div>
			</div>
			<div className="mb-5 flex justify-end">
				<Select
					value={semester}
					onValueChange={(value) => setSemester(value)}
				>
					<SelectTrigger className="w-[200px]">
						<SelectValue placeholder="Chọn niên khoá" />
					</SelectTrigger>
					<SelectContent className="w-full">
						<SelectGroup>
							{semesters?.map((i) => (
								<SelectItem
									value={i.semesterId}
									key={i.semesterId}
								>
									{i.semesterName}
								</SelectItem>
							))}
						</SelectGroup>
					</SelectContent>
				</Select>
			</div>

			<div>
				<CustomTable
					pageSize={pageSize}
					data={student}
					columns={columns}
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
};

export default StudyClassDetail;
