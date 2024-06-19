import { getTeacherDepartmentInfo } from "@/apis/teacher.info.api";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import Pagination from "@/components/pagination";
import { TableDetails } from "@/components/table/Table";
import { Input } from "@/components/ui/input";
import { useDebounce } from "@/hooks";
import { HeadDepartment } from "@/pages/teacherDepartment/HeadDepartment";
import { Notify } from "@/pages/teacherDepartment/Notify";
import { IAppState, useAppSelector } from "@/redux/store";
import { ITeacherInfo } from "@/types/teacher.info.type";
import { ColumnDef } from "@tanstack/react-table";
import { Search } from "lucide-react";
import { useEffect, useState } from "react";

const statusList = [
	{ value: "1", label: "Đang giảng dạy" },
	{ value: "2", label: "Tạm nghỉ" },
];

const columns: ColumnDef<ITeacherInfo>[] = [
	{
		accessorKey: "teacherId",
		header: "MSGV",
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
		accessorKey: "level",
		header: "Chức danh",
		cell: ({ row }) => <div>{row.getValue("level")}</div>,
	},
	{
		accessorKey: "status",
		header: "Tình trạng giảng dạy",
		cell: ({ row }) => <div>{row.getValue("status")}</div>,
		size: 500,
	},
];

const TeacherDepartment = () => {
	const { accoundId } = useAppSelector((state: IAppState) => state.users);
	const [searchValue, setSearchValue] = useState("");
	const [teachers, setTeachers] = useState<ITeacherInfo[]>([]);
	const [selectedField, setSelectedField] = useState<string[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);
	const searchQuery = useDebounce(searchValue, 1500);

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, searchQuery]);

	const handleGetData = () => {
		setLoading(true);
		getTeacherDepartmentInfo(
			{
				searchValue: searchQuery,
				pageSize: pageSize,
				pageNumber: pageNumber,
				status: selectedField?.map((i) => Number(i)),
			},
			accoundId!
		).then((res) => {
			setLoading(false);
			setTotalPage(res?.data?.totalPageCount);
			setTeachers(res?.data?.dataList);
		});
	};

	return (
		<>
			<div className="mb-4 text-2xl font-medium uppercase">Tổ bộ môn</div>
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
					placeholder="Tình trạng giảng dạy"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div className="flex justify-between gap-2">
					<HeadDepartment id={teachers?.[0]?.departmentId} />
					<Notify id={teachers?.[0]?.departmentId} />
				</div>
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={teachers}
					columns={columns}
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

export default TeacherDepartment;
