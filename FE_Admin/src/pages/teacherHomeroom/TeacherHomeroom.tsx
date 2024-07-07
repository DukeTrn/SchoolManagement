import { useEffect, useState } from "react";
import { ColumnDef } from "@tanstack/react-table";
import { IStudy } from "@/types/study.type";
import { Link } from "react-router-dom";
import { TableDetails } from "@/components/table/Table";
import { IAppState, useAppSelector } from "@/redux/store";
import { getHomeroomTeacher } from "@/apis/teacher.info.api";
import Pagination from "@/components/pagination";

export default function TeacherHomeroom() {
	const [loading, setLoading] = useState<boolean>(false);
	const { accoundId } = useAppSelector((state: IAppState) => state.users);
	const [info, setInfo] = useState<any>();
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);

	const columns: ColumnDef<IStudy>[] = [
		{
			accessorKey: "classId",
			header: "Tên lớp",
			cell: ({ row }) => {
				const className = row.original.className;
				return (
					<Link
						to={row.getValue("classId")}
						state={{ ...row.original }}
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
			accessorKey: "academicYear",
			header: "Niên khóa",
			cell: ({ row }) => <div>{row.getValue("academicYear")}</div>,
		},
	];

	const initValue = {
		pageSize: pageSize,
		pageNumber: pageNumber,
		searchValue: "",
	};

	useEffect(() => {
		setLoading(true);
		getHomeroomTeacher(accoundId!, initValue).then((res) => {
			setInfo(res?.data.dataList);
			setLoading(false);
		});
	}, []);

	return (
		<>
			<div className="mb-4 text-2xl font-medium">
				DANH SÁCH LỚP CHỦ NHIỆM
			</div>

			<div>
				<TableDetails
					pageSize={10}
					data={info ?? []}
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
