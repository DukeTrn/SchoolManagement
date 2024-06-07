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
		header: "Status",
		cell: ({ row }) => (
			<div className="capitalize">{row.getValue("status")}</div>
		),
	},
	{
		accessorKey: "email",
		header: () => {
			return <div>Email</div>;
		},
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "email1",
		header: () => {
			return <div>Email</div>;
		},
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "email2",
		header: () => {
			return <div>Email</div>;
		},
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "email3",
		header: () => {
			return <div>Email</div>;
		},
		cell: ({ row }) => (
			<div className="lowercase">{row.getValue("email")}</div>
		),
	},
	{
		accessorKey: "amount",
		header: () => <div>Amount</div>,
		cell: ({ row }) => {
			const amount = parseFloat(row.getValue("amount"));

			// Format the amount as a dollar amount
			const formatted = new Intl.NumberFormat("en-US", {
				style: "currency",
				currency: "USD",
			}).format(amount);

			return <div className="font-medium">{formatted}</div>;
		},
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

	const handleChange = (value: IAccount[]) => {
		setSelectedRows(value);
	};

	console.log("selectedRows", selectedRows);

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
				<div className="flex justify-between gap-5">
					<Button variant="outline">Kích hoạt</Button>
					<Button variant="outline">Ngừng hoạt động</Button>
					<Button variant="outline">Xóa</Button>
				</div>
				<div>
					<Button variant="outline">Xuất file</Button>
				</div>
			</div>
			<div>
				<TableDetails
					data={data}
					columns={columns}
					onChange={handleChange}
				/>
			</div>
		</>
	);
};

export default Account;
