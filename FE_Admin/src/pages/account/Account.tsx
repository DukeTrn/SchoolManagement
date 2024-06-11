import { useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import Pagination from "@/components/pagination";

const roles = [
	{ value: "0", label: "Học sinh" },
	{ value: "1", label: "Giáo viên" },
	{ value: "2", label: "Quản trị" },
];
export type IAccount = {
	accountId?: string;
	userName: string;
	fullName: string;
	password?: string;
	createdAt: string;
	modifiedAt?: string;
	isActive?: boolean;
	role: string;
};

export const columns: ColumnDef<IAccount>[] = [
	{
		id: "select",
		header: ({ table }) => (
			<Checkbox
				checked={
					table.getIsAllPageRowsSelected() ||
					(table.getIsSomePageRowsSelected() && "indeterminate")
				}
				onCheckedChange={(value) =>
					table.toggleAllPageRowsSelected(!!value)
				}
				aria-label="Select all"
			/>
		),
		cell: ({ row }) => (
			<Checkbox
				checked={row.getIsSelected()}
				onCheckedChange={(value) => row.toggleSelected(!!value)}
				aria-label="Select row"
			/>
		),
		enableSorting: false,
		enableHiding: false,
	},
	{
		accessorKey: "userName",
		header: "Tài khoản",
		cell: ({ row }) => <div>{row.getValue("userName")}</div>,
	},
	{
		accessorKey: "fullName",
		header: () => {
			return <div>Họ tên</div>;
		},
		cell: ({ row }) => <div>{row.getValue("fullName")}</div>,
	},
	{
		accessorKey: "role",
		header: () => {
			return <div>Chức vụ</div>;
		},
		cell: ({ row }) => <div>{row.getValue("role")}</div>,
	},
	{
		accessorKey: "createdAt",
		header: "Ngày tạo",
		cell: ({ row }) => <div>{row.getValue("createdAt")}</div>,
	},
	{
		accessorKey: "modifiedAt",
		header: "Ngày cập nhật",
		cell: ({ row }) => <div>{row.getValue("modifiedAt")}</div>,
	},
	{
		accessorKey: "isActive",
		header: "Tình trạng hoạt động",
		cell: ({ row }) => <div className="lowercase">{}</div>,
	},
];
let data: IAccount[] = [
	{
		userName: "m5gr84i9",
		fullName: "Le Vu Bao Trung",
		role: "Hoc sinh",
		createdAt: "01/01/2020",
		modifiedAt: "01/01/2020",
	},
	{
		userName: "m5gr384i9",
		fullName: "Le Vu Bao Trung",
		role: "Hoc sinh",
		createdAt: "01/01/2020",
		modifiedAt: "01/01/2020",
	},
	{
		userName: "m5gr284i9",
		fullName: "Le Vu Bao Trung",
		role: "Hoc sinh",
		createdAt: "01/01/2020",
		modifiedAt: "01/01/2020",
	},
	{
		userName: "m5gr584i9",
		fullName: "Le Vu Bao Trung",
		role: "Hoc sinh",
		createdAt: "01/01/2020",
		modifiedAt: "01/01/2020",
	},
	{
		userName: "m5gr584i9",
		fullName: "Le Vu Bao Trung",
		role: "Hoc sinh",
		createdAt: "01/01/2020",
		modifiedAt: "01/01/2020",
	},
	{
		userName: "m5gr584i9",
		fullName: "Le Vu Bao Trung",
		role: "Hoc sinh",
		createdAt: "01/01/2020",
		modifiedAt: "01/01/2020",
	},
	{
		userName: "m5gr584i9",
		fullName: "Le Vu Bao Trung",
		role: "Hoc sinh",
		createdAt: "01/01/2020",
		modifiedAt: "01/01/2020",
	},
];

const Account = () => {
	const [searchValue, setSearchValue] = useState("");
	const [position, setPosition] = useState("");
	const [selectedRows, setSelectedRows] = useState<IAccount[]>([]);
	const [selectedFields, setSelectedFields] = useState<string[]>([]);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);

	const handleChange = (value: IAccount[]) => {
		setSelectedRows(value);
	};

	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ TÀI KHOẢN</div>
			<div className="mb-5 flex justify-between">
				<div className="relative min-w-[295px]">
					<Search className="absolute left-2 top-2.5 h-4 w-4 " />
					<Input
						width={300}
						placeholder="Tìm kiếm"
						className="pl-8"
						onChange={(e) => setSearchValue(e.target?.value)}
					/>
				</div>
				<MultiSelect
					options={roles}
					onValueChange={setSelectedFields}
					defaultValue={selectedFields}
					placeholder="Tình trạng học tập"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div className="flex justify-between gap-2">
					<Button>Kích hoạt</Button>
					<Button>Ngừng hoạt động</Button>
					<Button>Xóa</Button>
				</div>
				<div>
					<Button>Xuất file</Button>
				</div>
			</div>
			<div>
				<TableDetails
					data={data}
					columns={columns}
					onChange={handleChange}
					// loading={true}
				/>
				<Pagination
					pageSize={pageSize}
					onChangePage={(value) => setPageNumber(Number(value))}
					onChangeRow={(value) => setPageSize(Number(value))}
				/>
			</div>
		</>
	);
};

export default Account;
