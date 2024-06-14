import {
	deleteTeacherDepartment,
	getDepartmentDetail,
} from "@/apis/department.api";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import Pagination from "@/components/pagination";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";
import { useDebounce } from "@/hooks";
import { ITeacher } from "@/types/teacher.type";
import { ColumnDef } from "@tanstack/react-table";
import { Search } from "lucide-react";
import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Create } from "./Create";

const statusList = [
	{ value: "1", label: "Đang dạy" },
	{ value: "2", label: "Tạm nghỉ" },
];

type ITeacherTable = Pick<
	ITeacher,
	| "teacherId"
	| "fullName"
	| "dob"
	| "phoneNumber"
	| "email"
	| "level"
	| "status"
	| "gender"
>;

const columns: ColumnDef<ITeacherTable>[] = [
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
		accessorKey: "teacherId",
		header: "MSBV",
		cell: ({ row }) => <div>{row.getValue("teacherId")}</div>,
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

const DepartmentDetail = () => {
	const { id } = useParams();
	const { toast } = useToast();
	const [searchValue, setSearchValue] = useState("");
	const [teachers, setTeachers] = useState<ITeacherTable[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [selectedField, setSelectedField] = useState<string[]>([]);
	const [selectedRows, setSelectedRows] = useState<ITeacherTable[]>([]);
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
		getDepartmentDetail(
			{
				pageNumber: pageNumber,
				pageSize: pageSize,
				searchValue: searchQuery,
				status: selectedField?.map((i) => Number(i)),
			},
			id as string
		).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setTeachers(res?.data?.dataList);
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

	const handleChange = (value: ITeacherTable[]) => {
		setSelectedRows(value);
	};

	const handleRemove = () => {
		deleteTeacherDepartment({
			departmentId: id as string,
			teacherIds: selectedRows?.map((i) => i.teacherId) as string[],
		}).then(() => {
			refreshData("Xoá giáo viên khỏi bộ môn thành công");
		});
	};

	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ BỘ MÔN </div>
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
					placeholder="Tình trạng giảng dạy"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
					handleRetrieve={handleGetData}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div className="flex gap-2">
					<Create
						departmentId={id as string}
						refreshData={refreshData}
					/>
					<Button onClick={handleRemove} disabled={isDisableButton}>
						Xóa
					</Button>
				</div>
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={teachers}
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

export default DepartmentDetail;
