import { TableDetails } from "@/components/table/Table";
import Pagination from "@/components/pagination";
import { IClass } from "@/types/study.type";
import { ColumnDef } from "@tanstack/react-table";
import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useDebounce } from "@/hooks";
import { Search, ArrowLeft } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { getClassDetail } from "@/apis/classroom.api";
import { Checkbox } from "@/components/ui/checkbox";
import { useToast } from "@/components/ui/use-toast";
import Conduct from "./Conduct";

export default function TeacherHomeroomDetail() {
	const location = useLocation();
	const navigation = useNavigate();
	const state = location?.state;
	const [student, setStudent] = useState<any>([]);
	const [semester] = useState<string>(`${state.academicYear.split(" ")[0]}1`);
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [searchValue, setSearchValue] = useState("");
	const [selectedRow, setSelectedRow] = useState<any>();
	const { toast } = useToast();

	const searchQuery = useDebounce(searchValue, 1500);
	const handleChange = (value: any) => {
		setSelectedRow(value?.[0]);
	};

	const renderStatus = (value: number) => {
		if (value === 1) return "Đang học";
		else return "Đình chỉ";
	};

	const columns: ColumnDef<IClass>[] = [
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
			accessorKey: "stt",
			header: () => {
				return <div>STT</div>;
			},
			cell: ({ row }) => <div>{row.index + 1}</div>,
			minSize: 200,
		},
		{
			accessorKey: "studentId",
			header: "MSHS",
			cell: ({ row }) => {
				const studentId = row.original.studentId;
				return (
					<Link
						to={row.getValue("studentId")}
						state={row.original}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{studentId}
					</Link>
				);
			},
		},
		{
			accessorKey: "fullName",
			header: () => {
				return <div>Họ Tên</div>;
			},
			cell: ({ row }) => <div>{row.getValue("fullName")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "status",
			header: "Tình trạng học tập",
			cell: ({ row }) => (
				<div>{renderStatus(row.getValue("status"))}</div>
			),
		},
	];

	const initValue = {
		searchValue: searchQuery,
		pageSize: pageSize,
		pageNumber: pageNumber,
		status: [1, 2],
	};
	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize, semester, searchQuery]);

	const handleGetData = () => {
		setLoading(true);
		getClassDetail(initValue, state.classId).then((res) => {
			setStudent(res.data?.dataList);
			setLoading(false);
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

	const isDisableButton = !selectedRow;

	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">{`DANH SÁCH HỌC SINH LỚP HỌC ${state.className}`}</div>
			</div>
			<div className="mb-5">
				<div className="flex gap-2">
					{/* <Create
						type="edit"
						disable={isDisableButton}
						selected={selectedRow}
						refreshData={refreshData}
					/> */}

					<Conduct
						selected={selectedRow}
						disable={isDisableButton}
						refreshData={refreshData}
					/>

					<Button
						disabled={isDisableButton}
						onClick={() => {
							navigation(
								`/teacher-homeroom/${state.classId}/${semester}`,
								{
									state: {
										semester: semester,
										grade: state.grade,
										classDetailId:
											selectedRow.classDetailId,
										fullName: selectedRow.fullName,
									},
								}
							);
						}}
					>
						Kết quả học tập
					</Button>
					<Button
						disabled={isDisableButton}
						onClick={() => {
							navigation(
								`/teacher-homeroom/${state.classId}/${state.academicYear}`,
								{
									state: {
										semester: semester,
										grade: state.grade,
										classDetailId:
											selectedRow.classDetailId,
										fullName: selectedRow.fullName,
									},
								}
							);
						}}
					>
						Tổng kết
					</Button>
				</div>
			</div>
			<div className="relative mb-5 max-w-[300px]">
				<Search className="absolute left-2 top-2.5 h-4 w-4 " />
				<Input
					width={300}
					placeholder="Tìm kiếm"
					className="pl-8"
					onChange={(e) => setSearchValue(e.target?.value)}
				/>
			</div>
			<div>
				<TableDetails
					pageSize={10}
					data={student ?? []}
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
}
