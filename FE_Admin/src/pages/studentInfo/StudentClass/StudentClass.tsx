import { useEffect, useState } from "react";
import { ColumnDef } from "@tanstack/react-table";
import { IStudy } from "@/types/study.type";
import { Link } from "react-router-dom";
import { TableDetails } from "@/components/table/Table";
import { StudentClassDetail } from "@/apis/student.api";
import { IAppState, useAppSelector } from "@/redux/store";
import { IStudent } from "@/types/student.type";

const StudentClass = () => {
	const [loading, setLoading] = useState<boolean>(false);
	const { accoundId } = useAppSelector((state: IAppState) => state.users);
	const [info, setInfo] = useState<IStudent>();

	const columns: ColumnDef<IStudy>[] = [
		{
			accessorKey: "classId",
			header: "Tên lớp",
			cell: ({ row }) => {
				const className = row.original.className;
				return (
					<Link
						to={row.getValue("classId")}
						state={row.original}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{className}
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
			accessorKey: "totalStudents",
			header: () => {
				return <div>Sĩ số</div>;
			},
			cell: ({ row }) => <div>{row.getValue("totalStudents")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "homeroomTeacherName",
			header: "GVCN",
			cell: ({ row }) => <div>{row.getValue("homeroomTeacherName")}</div>,
		},
		{
			accessorKey: "academicYear",
			header: "Niên khóa",
			cell: ({ row }) => <div>{row.getValue("academicYear")}</div>,
		},
	];

	useEffect(() => {
		setLoading(true);
		StudentClassDetail(accoundId!).then((res) => {
			setInfo(res?.data.data);
			setLoading(false);
		});
	}, []);

	return (
		<>
			<div className="mb-4 text-2xl font-medium">THÔNG TIN LỚP HỌC</div>

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
};
export default StudentClass;
