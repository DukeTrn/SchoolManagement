import { useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { MultiSelect } from "@/components/multiselect/MultiSelect";
import { StudentDetail } from "./StudentDetails";
import { IStudent } from "@/types/student.type";
import { Panel } from "./Create";

const statusList = [
	{ value: "0", label: "Đang học" },
	{ value: "1", label: "Đình chỉ" },
	{ value: "2", label: "Nghỉ  học" },
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
const data: IStudent[] = [
	{
		studentId: "20193153",
		fullName: "Le Vu Bao Trung",
		dob: "03/11/2001",
		gender: "Nam",
		phoneNumber: "0395311258",
		email: "trunglvb.hust@gmail.com",
		status: "Đang học",
	},
	{
		studentId: "20193154",
		fullName: "Le Vu Bao Trung",
		dob: "03/11/2001",
		gender: "Nam",
		phoneNumber: "0395311258",
		email: "trunglvb.hust@gmail.com",
		status: "Đang học",
	},
	{
		studentId: "20193155",
		fullName: "Le Vu Bao Trung",
		dob: "03/11/2001",
		gender: "Nam",
		phoneNumber: "0395311258",
		email: "trunglvb.hust@gmail.com",
		status: "Đang học",
	},
];

const Student = () => {
	const [searchValue, setSearchValue] = useState("");
	const [position, setPosition] = useState("");
	const [selectedRows, setSelectedRows] = useState<IStudent[]>([]);
	const [selectedField, setSelectedField] = useState<string[]>([]);

	const isDisableButton =
		selectedRows?.length > 1 || selectedRows?.length === 0;
	const handleChange = (value: IStudent[]) => {
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
					options={statusList}
					onValueChange={setSelectedField}
					placeholder="Tình trạng học tập"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div className="flex justify-between gap-2">
					<Panel type="create" disable={false} />
					<Panel
						type="edit"
						disable={isDisableButton}
						selectedStudent={selectedRows?.[0]}
					/>
					<Button disabled={isDisableButton}>Xóa</Button>
				</div>
				<div>
					<Button className="min-w-[100px]">Xuất file</Button>
				</div>
			</div>
			<div>
				<TableDetails
					data={data}
					columns={columns}
					onChange={handleChange}
					loading={false}
				/>
			</div>
		</>
	);
};

export default Student;
