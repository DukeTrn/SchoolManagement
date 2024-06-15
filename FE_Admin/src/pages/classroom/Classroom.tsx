import { useEffect, useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import { useDebounce } from "@/hooks";
import { useToast } from "@/components/ui/use-toast";
import Pagination from "@/components/pagination";
import { deleteClass, getClassroom } from "@/apis/classroom.api";
import { Create } from "./Create";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Link } from "react-router-dom";

const columns: ColumnDef<IClassroom>[] = [
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
		size: 10,
	},
	{
		accessorKey: "classId",
		header: "Tên lớp",
		cell: ({ row }) => {
			const className = row.original.className;

			return (
				<Link
					to={row.getValue("classId")}
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
		accessorKey: "homeroomTeacherName",
		header: "GVCN",
		cell: ({ row }) => <div>{row.getValue("homeroomTeacherName")}</div>,
	},
	{
		accessorKey: "academicYear",
		header: "Niên khoá",
		cell: ({ row }) => <div>{row.getValue("academicYear")}</div>,
	},
];

const Classroom = () => {
	const { toast } = useToast();
	const [classes, setClasses] = useState<IClassroom[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [searchValue, setSearchValue] = useState("");
	const [selectedRows, setSelectedRows] = useState<IClassroom[]>([]);
	const [selectedField, setSelectedField] = useState<string>("10");
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);

	const searchQuery = useDebounce(searchValue, 1500);

	const isDisableButton =
		selectedRows?.length > 1 || selectedRows?.length === 0;

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, searchQuery, selectedField]);

	const handleChange = (value: IClassroom[]) => {
		setSelectedRows(value);
	};

	const handleGetData = () => {
		setLoading(true);
		getClassroom(
			{
				searchValue: searchQuery,
				pageSize: pageSize,
				pageNumber: pageNumber,
			},
			Number(selectedField)
		).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setClasses(res?.data?.dataList);
		});
	};

	const refreshData = (message: string, varient?: boolean) => {
		setSelectedRows([]);
		handleGetData();
		toast({
			title: "Thông báo:",
			description: message,
			className: "border-2 border-green-500 p-4",
			variant: varient ? "destructive" : "default",
		});
	};

	const handleDelete = () => {
		deleteClass(selectedRows?.[0]?.classId)
			.then(() => {
				refreshData("Xóa lớp thành công!");
			})
			.catch(() => {
				toast({
					title: "Thông báo:",
					description: "Đã xảy ra lỗi",
					className: "border-2 border-green-500 p-4",
					variant: "destructive",
				});
			});
	};

	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ LỚP HỌC</div>
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
			<div className="mb-5 flex justify-between">
				<div className="flex justify-between gap-2">
					<Create
						type="create"
						disable={false}
						refreshData={refreshData}
					/>
					<Create
						type="edit"
						disable={isDisableButton}
						selected={selectedRows?.[0]}
						refreshData={refreshData}
					/>
					<Button disabled={isDisableButton} onClick={handleDelete}>
						Xóa
					</Button>
				</div>
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={classes}
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

export default Classroom;
