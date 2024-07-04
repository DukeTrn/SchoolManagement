import { deleteClassDetail, getClassDetail } from "@/apis/classroom.api";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import Pagination from "@/components/pagination";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";
import { useDebounce } from "@/hooks";
import { StudentDetail } from "@/pages/student/StudentDetails";
import { IClassroom, IStudentClassroom } from "@/types/classroom.type";
import { ColumnDef } from "@tanstack/react-table";
import { Search, ArrowLeft } from "lucide-react";
import { useEffect, useState } from "react";
import { useLocation, useParams, useNavigate } from "react-router-dom";
import { Create } from "./Create";
import { Edit } from "./Edit";
import DeleteConfirm from "@/components/deleteConfirm";

const statusList = [
	{ value: "1", label: "Đang học" },
	{ value: "2", label: "Đình chỉ" },
	{ value: "3", label: "Nghỉ  học" },
];

const columns: ColumnDef<IStudentClassroom>[] = [
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
		header: "Họ tên",
		cell: ({ row }) => <div>{row.getValue("fullName")}</div>,
	},
	{
		accessorKey: "status",
		header: "Tình trạng học tập",
		cell: ({ row }) => (
			<div>
				{
					statusList?.find((i) => i.value == row.getValue("status"))
						?.label
				}
			</div>
		),
		size: 600,
	},
];

const ClassroomDetail = () => {
	const location = useLocation();
	const state: IClassroom = location?.state;
	const navigation = useNavigate();

	const { id } = useParams();
	const { toast } = useToast();
	const [searchValue, setSearchValue] = useState("");
	const [students, setStudents] = useState<IStudentClassroom[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [selectedField, setSelectedField] = useState<string[]>([]);
	const [selectedRow, setSelectedRow] = useState<IStudentClassroom>();
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage, setTotalPage] = useState<number>(1);

	const searchQuery = useDebounce(searchValue, 1500);
	const isDisableButton = !selectedRow;

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, searchQuery, id]);

	const handleGetData = () => {
		setLoading(true);
		getClassDetail(
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
			setStudents(res?.data?.dataList);
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

	const handleChange = (value: IStudentClassroom[]) => {
		setSelectedRow(value?.[0]);
	};

	const handleRemove = () => {
		deleteClassDetail(selectedRow?.classDetailId!).then(() => {
			refreshData("Xoá học sinh khỏi lớp học thành công");
		});
	};

	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">
					QUẢN LÝ HỌC SINH LỚP {location.state.className}
				</div>
			</div>

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
					placeholder="Tình trạng học tập"
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
						id={id as string}
						refreshData={refreshData}
						state={state}
					/>
					<Edit
						id={id as string}
						refreshData={refreshData}
						state={state}
						disable={!selectedRow}
					/>
					<DeleteConfirm
						disabled={isDisableButton}
						onClick={handleRemove}
					/>
				</div>
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={students}
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

export default ClassroomDetail;
