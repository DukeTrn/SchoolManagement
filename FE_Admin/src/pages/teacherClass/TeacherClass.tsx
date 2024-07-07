import { useEffect, useState } from "react";
import { ColumnDef } from "@tanstack/react-table";
import { IStudy } from "@/types/study.type";
import { Link } from "react-router-dom";
import { TableDetails } from "@/components/table/Table";
import { IAppState, useAppSelector } from "@/redux/store";
import { getTeacherClass } from "@/apis/teacher.info.api";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { IFilterSemesters } from "@/types/semester.type";
import { getAllFilterSemester } from "@/apis/semester.api";

export default function TeacherClass() {
	const [loading, setLoading] = useState<boolean>(false);
	const { accoundId } = useAppSelector((state: IAppState) => state.users);
	const [info, setInfo] = useState<any>();
	const [pageSize] = useState<number>(10);
	const [pageNumber] = useState<number>(1);
	const [semester, setSemester] = useState<string>("");
	const [semesters, setSemesters] = useState<IFilterSemesters[]>([]);

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
		getAllFilterSemester().then((res) => {
			setSemesters(res?.data?.data);
			setSemester(res?.data?.data[0]?.semesterId);
		});
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
							{semesters?.map((i) => (
								<SelectItem
									value={i.semesterId}
									key={i.semesterName}
								>
									{i.semesterId}
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
