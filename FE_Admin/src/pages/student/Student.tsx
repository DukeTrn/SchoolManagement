import { useEffect, useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import { StudentDetail } from "./StudentDetails";
import { IStudent, IStudentInfo } from "@/types/student.type";
import { Panel } from "./Create";
import { useDebounce } from "@/hooks";
import {
	deleteStudent,
	exportStudent,
	getAllStudent,
} from "@/apis/student.api";
import { useToast } from "@/components/ui/use-toast";
import Pagination from "@/components/pagination";
import { downloadFile } from "@/utils/utils";

const statusList = [
	{ value: "1", label: "Đang học" },
	{ value: "2", label: "Đình chỉ" },
	{ value: "3", label: "Nghỉ  học" },
];

const columns: ColumnDef<IStudent>[] = [
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
		accessorKey: "studentId",
		header: "MSHS",
		cell: ({ row }) => (
			<StudentDetail studentId={row.getValue("studentId")} />
		),
	},
	{
		accessorKey: "fullName",
		header: () => {
			return <div>Họ tên</div>;
		},
		cell: ({ row }) => <div>{row.getValue("fullName")}</div>,
		minSize: 200,
	},
	{
		accessorKey: "dob",
		header: "Ngày sinh",
		cell: ({ row }) => <div>{row.getValue("dob")}</div>,
	},
	{
		accessorKey: "gender",
		header: "Giới tính",
		cell: ({ row }) => <div>{row.getValue("gender")}</div>,
	},
	{
		accessorKey: "phoneNumber",
		header: "SDT",
		cell: ({ row }) => <div>{row.getValue("phoneNumber")}</div>,
	},
	{
		accessorKey: "email",
		header: "Email",
		cell: ({ row }) => <div>{row.getValue("email")}</div>,
	},
	{
		accessorKey: "status",
		header: "Tình trạng học tập",
		cell: ({ row }) => <div>{row.getValue("status")}</div>,
		size: 500,
	},
];

const Student = () => {
	const { toast } = useToast();
	const [students, setStudents] = useState<IStudentInfo[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [searchValue, setSearchValue] = useState("");
	const [selectedRows, setSelectedRows] = useState<IStudent[]>([]);
	const [selectedField, setSelectedField] = useState<string[]>([]);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);
	const [loadingExport, setLoadingExport] = useState<boolean>(false);

	const searchQuery = useDebounce(searchValue, 1500);

	const isDisableButton =
		selectedRows?.length > 1 || selectedRows?.length === 0;

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, searchQuery]);

	const handleChange = (value: IStudent[]) => {
		setSelectedRows(value);
	};

	const handleGetData = () => {
		setLoading(true);
		getAllStudent({
			searchValue: searchQuery,
			pageSize: pageSize,
			pageNumber: pageNumber,
			status: selectedField?.map((i) => Number(i)),
		}).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setStudents(res?.data?.dataList);
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

	const handleDelete = () => {
		deleteStudent(selectedRows?.[0]?.studentId as string).then(() => {
			refreshData("Xóa học sinh thành công!");
		});
	};

	const handleExport = () => {
		setLoadingExport(true);
		exportStudent({
			studentIds:
				(selectedRows?.map((item) => item.studentId) as string[]) ?? [],
			status: [],
		}).then((res) => {
			setLoadingExport(false);
			downloadFile(res?.data, "QuanLyHocSinh");
		});
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
					options={statusList}
					onValueChange={setSelectedField}
					handleRetrieve={handleGetData}
					value={selectedField}
					placeholder="Tình trạng học tập"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div className="flex justify-between gap-2">
					<Panel
						type="create"
						disable={false}
						refreshData={refreshData}
					/>
					<Panel
						type="edit"
						disable={isDisableButton}
						selectedStudent={selectedRows?.[0]}
						refreshData={refreshData}
					/>
					<Button disabled={isDisableButton} onClick={handleDelete}>
						Xóa
					</Button>
				</div>
				<div>
					<Button
						className="w-[100px]"
						onClick={handleExport}
						loading={loadingExport}
					>
						Xuất file
					</Button>
				</div>
			</div>
			<div>
				<TableDetails
					data={students}
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

export default Student;
