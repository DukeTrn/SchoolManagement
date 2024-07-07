import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { useEffect, useState } from "react";
import { getStudyInfo } from "@/apis/study.api";
import { ColumnDef } from "@tanstack/react-table";
import { IStudy } from "@/types/study.type";
import { Link } from "react-router-dom";
import { TableDetails } from "@/components/table/Table";
import Pagination from "@/components/pagination";

const academicYears = [
	"2019 - 2020",
	"2020 - 2021",
	"2021 - 2022",
	"2022 - 2023",
	"2023 - 2024",
	"2024 - 2025",
];

const Study = () => {
	const [selectedField, setSelectedField] = useState<string>("10");
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [year, setYear] = useState<string>("2019 - 2020");
	const [classroom, setClassroom] = useState<
		{ classId: string; className: string; homeroomTeacherName: string }[]
	>([]);

	const columns: ColumnDef<IStudy>[] = [
		{
			accessorKey: "classId",
			header: "Tên lớp",
			cell: ({ row }) => {
				const className = row.original.className;
				return (
					<Link
						to={row.getValue("classId")}
						state={{ ...row.original, year, selectedField }}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{className}
					</Link>
				);
			},
		},
		{
			accessorKey: "description",
			header: () => {
				return <div>Mô tả</div>;
			},
			cell: ({ row }) => <div>{row.getValue("description")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "homeroomTeacherName",
			header: "GVCN",
			cell: ({ row }) => <div>{row.getValue("homeroomTeacherName")}</div>,
		},
	];

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, selectedField, year]);

	const handleGetData = () => {
		setLoading(true);
		getStudyInfo(year, Number(selectedField)).then((res) => {
			setClassroom(res?.data?.data);
			setLoading(false);
		});
	};

	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ HỌC TẬP</div>
			<div className="mb-5 flex justify-end gap-5">
				<Select value={year} onValueChange={(value) => setYear(value)}>
					<SelectTrigger className="w-[200px]">
						<SelectValue placeholder="Chọn niên khoá" />
					</SelectTrigger>
					<SelectContent className="w-full">
						<SelectGroup>
							{academicYears?.map((value) => (
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
					data={classroom}
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
export default Study;
