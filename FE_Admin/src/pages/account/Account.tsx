import { useEffect, useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import Pagination from "@/components/pagination";
import { IAccount } from "@/types/account.type";
import {
	activeAccount,
	deleteAccount,
	getAllAccount,
} from "@/apis/account.api";
import { useToast } from "@/components/ui/use-toast";
import { ToastAction } from "@/components/ui/toast";
import { useDebounce } from "@/hooks";

const roles = [
	{ value: "1", label: "Admin" },
	{ value: "2", label: "Giáo viên chủ nhiệm" },
	{ value: "3", label: "Giáo viên" },
	{ value: "4", label: "Học sinh" },
];

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
		size: 30,
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
		cell: ({ row }) => (
			<div>
				{row.getValue("isActive")
					? "Đang hoạt động"
					: "Ngừng hoạt động"}
			</div>
		),
		minSize: 200,
	},
];

const Account = () => {
	const { toast } = useToast();
	const [loading, setLoading] = useState<boolean>(false);
	const [accounts, setAccounts] = useState<IAccount[]>([]);
	const [searchValue, setSearchValue] = useState("");
	const [selectedRows, setSelectedRows] = useState<IAccount[]>([]);
	const [selectedFields, setSelectedFields] = useState<string[]>([]);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);

	const searchQuery = useDebounce(searchValue, 1500);

	const isDisableButton =
		selectedRows?.length <= 0 || selectedRows?.length > 1;

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, searchQuery]);

	const handleGetData = () => {
		setLoading(true);
		getAllAccount({
			searchValue: searchQuery,
			pageSize: pageSize,
			pageNumber: pageNumber,
			roles: selectedFields?.map((i) => Number(i)),
		}).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setAccounts(res?.data?.dataList);
		});
	};

	const refreshData = (message: string) => {
		setSelectedRows([]);
		handleGetData();
		toast({
			title: "Thông báo:",
			description: message,
			className: "border-2 border-green-500 p-4",
		});
	};

	const handleActive = (isActive: boolean) => {
		activeAccount({
			accountId: selectedRows?.[0].accountId,
			isActive: isActive,
		}).then((res) => {
			refreshData(res?.data?.message);
		});
	};

	const handleDelete = () => {
		deleteAccount(selectedRows[0]?.accountId as string).then((res) => {
			refreshData(res?.data?.message);
		});
	};

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
					handleRetrieve={handleGetData}
					defaultValue={selectedFields}
					placeholder="Chức vụ"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div className="flex justify-between gap-2">
					<Button
						onClick={() => handleActive(true)}
						disabled={isDisableButton || selectedRows?.[0].isActive}
					>
						Kích hoạt
					</Button>
					<Button
						onClick={() => handleActive(false)}
						disabled={
							isDisableButton || !selectedRows?.[0].isActive
						}
					>
						Ngừng hoạt động
					</Button>
					<Button disabled={isDisableButton} onClick={handleDelete}>
						Xóa
					</Button>
				</div>
			</div>
			<div>
				<TableDetails
					data={accounts}
					columns={columns}
					onChange={handleChange}
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

export default Account;
