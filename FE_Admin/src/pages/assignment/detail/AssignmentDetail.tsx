import { useEffect, useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { useDebounce } from "@/hooks";
import { useToast } from "@/components/ui/use-toast";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { IAssignmentDisplay } from "@/types/assignment.type";
import {
	deleteAssignment,
	getTeacherAssignment,
	updateAssignment,
} from "@/apis/asignment.api";
import { useLocation, useParams, useNavigate } from "react-router-dom";
import { getAllSemesters } from "@/apis/semester.api";
import { Create } from "./Create";
import { ArrowLeft } from "lucide-react";

const columns: ColumnDef<IAssignmentDisplay>[] = [
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
		size: 10,
	},
	{
		accessorKey: "teacherName",
		header: "Họ tên",
		cell: ({ row }) => <div>{row.getValue("teacherName")}</div>,
	},
	{
		accessorKey: "email",
		header: () => {
			return <div>Email</div>;
		},
		cell: ({ row }) => <div>{row.getValue("email")}</div>,
	},
	{
		accessorKey: "phoneNumber",
		header: "SĐT",
		cell: ({ row }) => <div>{row.getValue("phoneNumber")}</div>,
	},
	{
		accessorKey: "className",
		header: "Lớp học",
		cell: ({ row }) => <div>{row.getValue("className")}</div>,
	},
	{
		accessorKey: "semesterName",
		header: "Học kỳ",
		cell: ({ row }) => <div>{row.getValue("semesterName")}</div>,
	},
	{
		accessorKey: "academicYear",
		header: "Niên khoá",
		cell: ({ row }) => <div>{row.getValue("academicYear")}</div>,
	},
];

const AssignmentDetails = () => {
	const { id } = useParams();
	const location = useLocation();
	const navigation = useNavigate();
	const state = location?.state;
	const { toast } = useToast();
	const [teachers, setTeachers] = useState<IAssignmentDisplay[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [searchValue, setSearchValue] = useState("");
	const [selectedRow, setSelectedRow] = useState<IAssignmentDisplay>();
	const [selectedField, setSelectedField] = useState<string>();
	const [list, setList] = useState<{ label: string; value: string }[]>([]);

	const searchQuery = useDebounce(searchValue, 1500);

	const isDisableButton = !selectedRow;

	useEffect(() => {
		if (selectedField) {
			handleGetData();
		}
	}, [searchQuery, selectedField]);

	useEffect(() => {
		getAllSemesters({ pageSize: 50, pageNumber: 1 }).then((res) => {
			const data = res?.data?.dataList?.map((i) => ({
				label: i.semesterName,
				value: i.semesterId,
			}));
			setList(data);
			setSelectedField(data?.[0]?.value);
		});
	}, []);

	const handleChange = (value: IAssignmentDisplay[]) => {
		setSelectedRow(value?.[0]);
	};

	const handleGetData = () => {
		setLoading(true);
		getTeacherAssignment(
			{
				searchValue: searchQuery,
			},
			Number(state?.selectedField),
			state?.id,
			selectedField!
		).then((res) => {
			setLoading(false);
			setTeachers(res?.data?.data);
		});
	};

	const refreshData = (message: string, varient?: boolean) => {
		setSelectedRow(undefined);
		handleGetData();
		toast({
			title: "Thông báo:",
			description: message,
			className: "border-2 border-green-500 p-4",
			variant: varient ? "destructive" : "default",
		});
	};

	const handleDelete = () => {
		deleteAssignment(selectedRow?.assignmentId!)
			.then(() => {
				refreshData("Xóa phân công giảng dạy thành công!");
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
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium uppercase">
					QUẢN LÝ PHÂN CÔNG GIẢNG DẠY môn{" "}
					{location?.state.subjectName}
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
				<Select
					value={selectedField}
					onValueChange={(value) => setSelectedField(value)}
				>
					<SelectTrigger className="w-[200px]">
						<SelectValue placeholder="Học kỳ" />
					</SelectTrigger>
					<SelectContent className="w-full">
						<SelectGroup>
							{list?.map((i) => (
								<SelectItem value={i.value} key={i.value}>
									{i.label}
								</SelectItem>
							))}
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
						academicYear={selectedField as string}
						grade={Number(state?.selectedField)}
						semesterId={selectedField!}
						subjectId={id!}
					/>
					<Button
						disabled={isDisableButton}
						onClick={() => {
							updateAssignment(
								selectedRow?.assignmentId!,
								selectedRow?.classId!
							).then(() => {
								refreshData("Cập nhật giáo viên thành công!");
							});
						}}
					>
						Chuyển lớp
					</Button>
					<Button disabled={isDisableButton} onClick={handleDelete}>
						Xóa
					</Button>
				</div>
			</div>
			<div>
				<TableDetails
					pageSize={10}
					data={teachers}
					columns={columns}
					onChange={handleChange}
					loading={loading}
					useRadio
				/>
			</div>
		</>
	);
};

export default AssignmentDetails;
