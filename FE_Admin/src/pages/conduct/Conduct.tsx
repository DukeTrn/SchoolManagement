import { getConductInfo } from "@/apis/conduct.api";
import { IConduct } from "@/types/conduct.type";
import { ColumnDef } from "@tanstack/react-table";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { TableDetails } from "@/components/table/Table";
import Pagination from "@/components/pagination";

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

const Conduct = () => {
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);
	const [loading, setLoading] = useState<boolean>(false);
	const [selectedField, setSelectedField] = useState<string>("10");
	const [semester, setSemester] = useState<string>("20191");
	const [conduct, setConduct] = useState<any>([]);

	const columns: ColumnDef<IConduct>[] = [
		{
			accessorKey: "classId",
			header: "Tên lớp",
			cell: ({ row }) => {
				const className = row.original.className;
				return (
					<Link
						to={row.getValue("classId")}
						state={{ ...row.original, semester, selectedField }}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{className}
					</Link>
				);
			},
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
		handleGetData();
	}, [pageNumber, pageSize, selectedField, semester]);

	const handleGetData = () => {
		setLoading(true);
		getConductInfo(Number(selectedField), semester).then((res) => {
			setConduct(res?.data?.data);
			setLoading(false);
		});
	};

	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ HỌC TẬP</div>
			<div className="mb-5 flex justify-end gap-5">
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

				<Select
					value={selectedField}
					onValueChange={(value) => setSelectedField(value)}
				>
					<SelectTrigger className="w-[200px]">
						<SelectValue placeholder="Chọn trạng thái" />
					</SelectTrigger>
					<SelectContent className="w-full">
						<SelectGroup>
							<SelectItem value="10">Khối 10</SelectItem>
							<SelectItem value="11">Khối 11</SelectItem>
							<SelectItem value="12">Khối 12</SelectItem>
						</SelectGroup>
					</SelectContent>
				</Select>
			</div>

			<div>
				<TableDetails
					pageSize={pageSize}
					data={conduct}
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
};

export default Conduct;
