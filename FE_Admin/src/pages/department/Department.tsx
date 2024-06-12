import { TableDetails } from "@/components/table/Table";
import { Checkbox } from "@/components/ui/checkbox";
import { Button } from "@/components/ui/button";
import { ColumnDef } from "@tanstack/react-table";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Create } from "@/pages/department/Create";
import { IDepartment } from "@/types/department.type";
import { deleteDepartment, getAllDepartment } from "@/apis/department.api";
import Pagination from "@/components/pagination";
import { useToast } from "@/components/ui/use-toast";

const columns: ColumnDef<IDepartment>[] = [
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
		accessorKey: "departmentId",
		header: () => {
			return <div>ID</div>;
		},
		cell: ({ row }) => (
			<Link
				to={row.getValue("departmentId")}
				className="cursor-pointer font-medium text-blue-600 underline"
			>
				{row.getValue("departmentId")}
			</Link>
		),
	},
	{
		accessorKey: "subjectName",
		header: "Tổ bộ môn",
		cell: ({ row }) => <div>{row.getValue("subjectName")}</div>,
	},
	{
		accessorKey: "description",
		header: "Mô tả",
		cell: ({ row }) => <div>{row.getValue("description")}</div>,
	},
	{
		accessorKey: "notification",
		header: "Thông báo",
		cell: ({ row }) => <div>{row.getValue("notification")}</div>,
	},
];

const Department = () => {
	const { toast } = useToast();
	const [loading, setLoading] = useState<boolean>(false);
	const [selectedRow, setSelectedRow] = useState<IDepartment>();
	const [departments, setDepartments] = useState<IDepartment[]>([]);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);

	const handleChange = (value: IDepartment[]) => {
		setSelectedRow(value?.[0]);
	};

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize]);

	const handleGetData = () => {
		setLoading(true);
		getAllDepartment({
			pageNumber: pageNumber,
			pageSize: pageSize,
		}).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setDepartments(res?.data?.dataList);
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
		deleteDepartment(selectedRow?.departmentId as string).then(() => {
			refreshData("Xóa học sinh thành công!");
		});
	};

	const isDisableButton = !selectedRow;
	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ TỔ BỘ MÔN</div>
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
					data={departments}
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

export default Department;
