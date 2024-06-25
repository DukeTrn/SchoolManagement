import { useEffect, useState } from "react";
import { ColumnDef } from "@tanstack/react-table";
import { IStudy } from "@/types/study.type";
import { Link } from "react-router-dom";
import { TableDetails } from "@/components/table/Table";
import { StudentClassDetail } from "@/apis/student.api";
import { IAppState, useAppSelector } from "@/redux/store";
import { IStudent } from "@/types/student.type";
import { getTeacherClass } from "@/apis/teacher.info.api";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { ITeacherInfo } from "@/types/teacher.type";

const Semester = [
	"20171",
	"20172",
	"20181",
	"20182",
	"20191",
	"20192",
	"20201",
	"20202",
	"20211",
	"20212",
	"20221",
	"20222",
	"20231",
	"20232",
	"20241",
	"20242",
	"20251",
	"20252",
];

export default function TeacherClass() {
	const [loading, setLoading] = useState<boolean>(false);
	const { accoundId } = useAppSelector((state: IAppState) => state.users);
	const [info, setInfo] = useState<any>();
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [semester, setSemester] = useState<string>("20231");

	const columns: ColumnDef<IStudy>[] = [
		{
			accessorKey: "classId",
			header: "Tên lớp",
			cell: ({ row }) => {
				const className = row.original.className;
				return (
					<Link
						to={row.getValue("classId")}
						state={{ ...row.original, semester }}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{className}
					</Link>
				);
			},
		},
		{
			accessorKey: "subjectName",
			header: () => {
				return <div>Môn học</div>;
			},
			cell: ({ row }) => <div>{row.getValue("subjectName")}</div>,
			minSize: 200,
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
			accessorKey: "totalStudents",
			header: () => {
				return <div>Sĩ số</div>;
			},
			cell: ({ row }) => <div>{row.getValue("totalStudents")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "academicYear",
			header: "Niên khóa",
			cell: ({ row }) => <div>{row.getValue("academicYear")}</div>,
		},
	];

	const initValue = {
		pageSize: pageSize,
		pageNumber: pageNumber,
	};

	useEffect(() => {
		setLoading(true);
		getTeacherClass(accoundId!, semester, initValue).then((res) => {
			setInfo(res?.data.dataList);
			setLoading(false);
		});
	}, [semester]);

	return (
		<>
			<div className="mb-4 text-2xl font-medium">
				DANH SÁCH LỚP GIẢNG DẠY
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
							{Semester?.map((value) => (
								<SelectItem value={value} key={value}>
									{value}
								</SelectItem>
							))}
						</SelectGroup>
					</SelectContent>
				</Select>
			</div>

			<div>
				<TableDetails
					pageSize={10}
					data={info ?? []}
					columns={columns}
					//onChange={handleChange}
					loading={loading}
				/>
			</div>
		</>
	);
}
