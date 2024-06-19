import { getSemesterDetails, removeSemesterDetail } from "@/apis/semester.api";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import Pagination from "@/components/pagination";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";
import { useDebounce } from "@/hooks";
import { ColumnDef } from "@tanstack/react-table";
import { Search } from "lucide-react";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Create } from "./Create";

const statusList = [
	{ value: "10", label: "Khối 10" },
	{ value: "11", label: "Khối 11" },
	{ value: "12", label: "Khối 12" },
];

type ISemesterTable = Omit<ISemesterDetail, "academicYear">;

const columns: ColumnDef<ISemesterTable>[] = [
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
		accessorKey: "className",
		header: "Tên lớp",
		cell: ({ row }) => <div>{row.getValue("className")}</div>,
	},
	{
		accessorKey: "grade",
		header: () => {
			return <div>Khối</div>;
		},
		cell: ({ row }) => <div>{row.getValue("grade")}</div>,
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

const SemesterDetail = () => {
	const { id } = useParams();
	const { toast } = useToast();
	const [searchValue, setSearchValue] = useState("");
	const [semesters, setSemesters] = useState<ISemesterTable[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [selectedField, setSelectedField] = useState<string[]>([]);
	const [selectedRows, setSelectedRows] = useState<ISemesterTable[]>([]);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);

	const searchQuery = useDebounce(searchValue, 1500);
	const isDisableButton = selectedRows?.length === 0;

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, searchQuery, id]);

	const handleGetData = () => {
		setLoading(true);
		getSemesterDetails(
			{
				pageNumber: pageNumber,
				pageSize: pageSize,
				searchValue: searchQuery,
				grades: selectedField?.map((i) => Number(i)),
			},
			id as string
		).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setSemesters(res?.data?.dataList);
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

	const handleChange = (value: ISemesterTable[]) => {
		setSelectedRows(value);
	};

	const handleRemove = () => {
		removeSemesterDetail(selectedRows?.map((i) => i?.id!)).then(() => {
			refreshData("Xoá lớp khỏi học kỳ thành công");
		});
	};

	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ HỌC KỲ {id}</div>
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
					value={selectedField}
					placeholder="Khối"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
					handleRetrieve={handleGetData}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div className="flex gap-2">
					<Create id={id as string} refreshData={refreshData} />
					<Button onClick={handleRemove} disabled={isDisableButton}>
						Xóa
					</Button>
				</div>
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={semesters}
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

export default SemesterDetail;
