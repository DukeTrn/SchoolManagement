import { useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { Cat, Dog, Fish, Rabbit, Turtle } from "lucide-react";

export type IAccount = {
	id: string;
	amount: number;
	status: "pending" | "processing" | "success" | "failed";
	email: string;
	size?: number;
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
		accessorKey: "status",
		header: "Tài khoản",
		cell: ({ row }) => (
			<div className="capitalize">{row.getValue("status")}</div>
		),
	},
	{
		accessorKey: "email",
		header: () => {
			return <div>Họ tên</div>;
		},
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "email1",
		header: () => {
			return <div>Chức vụ</div>;
		},
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "email2",
		header: "Email",
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "email3",
		header: "Ngày tạo",
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "amount",
		header: "Ngày cập nhập",
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("amount")}</div>
		),
	},
];
const data: IAccount[] = [
	{
		id: "m5gr84i9",
		amount: 316,
		status: "success",
		email: "ken99@yahoo.com",
	},
	{
		id: "3u1reuv4",
		amount: 242,
		status: "success",
		email: "Abe45@gmail.com",
	},
	{
		id: "derv1ws0",
		amount: 837,
		status: "processing",
		email: "Monserrat44@gmail.com",
	},
	{
		id: "5kma53ae",
		amount: 874,
		status: "success",
		email: "Silas22@gmail.com",
	},
	{
		id: "bhqecj4p",
		amount: 721,
		status: "failed",
		email: "carmella@hotmail.com",
	},
];

const Account = () => {
	const [searchValue, setSearchValue] = useState("");
	const [position, setPosition] = useState("");
	const [selectedRows, setSelectedRows] = useState<IAccount[]>([]);
	const [selectedFrameworks, setSelectedFrameworks] = useState<string[]>([]);

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
				<div className="min-w-[200px]">
					<Select
						onValueChange={(value) => setPosition(value)}
						defaultValue=""
					>
						<SelectTrigger>
							<SelectValue placeholder="Chức vụ" />
						</SelectTrigger>

						<SelectContent>
							<SelectItem value="hs">Học sinh</SelectItem>
							<SelectItem value="gv">Giáo viên</SelectItem>
							<SelectItem value="admin">Quản trị viên</SelectItem>
						</SelectContent>
					</Select>
				</div>
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
					loading={true}
				/>
			</div>
		</>
	);
};

export default Account;
