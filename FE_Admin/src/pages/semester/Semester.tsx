import { TableDetails } from "@/components/table/Table";
import { Checkbox } from "@/components/ui/checkbox";
import { Button } from "@/components/ui/button";
import { ColumnDef } from "@tanstack/react-table";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Pagination from "@/components/pagination";
import { useToast } from "@/components/ui/use-toast";
import { deleteSemester, getAllSemesters } from "@/apis/semester.api";
import { Create } from "./Create";

const columns: ColumnDef<ISemester>[] = [
	{
		id: "select",
		header: "",
		cell: ({ row }) => (
			<Checkbox
				className="rounded-full"
				checked={row.getIsSelected()}
				onCheckedChange={(value) => row.toggleSelected(!!value)}
				aria-label="Select row"
			/>
		),
		enableSorting: false,
		enableHiding: false,
		size: 10,
	},
	{
		accessorKey: "semesterId",
		header: () => {
			return <div>Mã học kỳ</div>;
		},
		cell: ({ row }) => (
			<Link
				to={row.getValue("semesterId")}
				className="cursor-pointer font-medium text-blue-600 underline"
			>
				{row.getValue("semesterId")}
			</Link>
		),
	},
	{
		accessorKey: "semesterName",
		header: "Tên học kỳ",
		cell: ({ row }) => <div>{row.getValue("semesterName")}</div>,
	},
	{
		accessorKey: "academicYear",
		header: "Niên khoá",
		cell: ({ row }) => <div>{row.getValue("academicYear")}</div>,
	},
	{
		accessorKey: "timeStart",
		header: "Thời gian bắt đầu",
		cell: ({ row }) => <div>{row.getValue("timeStart")}</div>,
	},
	{
		accessorKey: "timeEnd",
		header: "Thời gian kết thúc",
		cell: ({ row }) => <div>{row.getValue("timeEnd")}</div>,
	},
];

const Semester = () => {
	const { toast } = useToast();
	const [loading, setLoading] = useState<boolean>(false);
	const [selectedRow, setSelectedRow] = useState<ISemester>();
	const [semesters, setSemesters] = useState<ISemester[]>([]);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);

	const handleChange = (value: ISemester[]) => {
		setSelectedRow(value?.[0]);
	};

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize]);

	const handleGetData = () => {
		setLoading(true);
		getAllSemesters({
			pageNumber: pageNumber,
			pageSize: pageSize,
		}).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setSemesters(res?.data?.dataList);
		});
	};

	const refreshData = (message: string) => {
		setSelectedRow(undefined);
		handleGetData();
		toast({
			title: "Thông báo:",
			description: message,
			className: "border-2 border-green-500 p-4",
		});
	};

	const handleDelete = () => {
		deleteSemester(selectedRow?.semesterId as string).then(() => {
			refreshData("Xóa học kỳ thành công!");
		});
	};

	const isDisableButton = !selectedRow;
	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ HỌC KỲ</div>
			<div className="mb-5">
				<div className="flex gap-2">
					<Create
						type="create"
						disable={false}
						refreshData={refreshData}
					/>
					<Create
						type="edit"
						disable={isDisableButton}
						selected={selectedRow}
						refreshData={refreshData}
					/>
					<Button disabled={isDisableButton} onClick={handleDelete}>
						Xóa
					</Button>
				</div>
			</div>
			<div className="mb-5">
				<TableDetails
					pageSize={pageSize}
					data={semesters}
					columns={columns}
					onChange={handleChange}
					loading={loading}
					useRadio
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

export default Semester;
