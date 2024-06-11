import { TableDetails } from "@/components/table/Table";
import { Checkbox } from "@/components/ui/checkbox";
import { Button } from "@/components/ui/button";
import { ColumnDef } from "@tanstack/react-table";
import { useState } from "react";
import { Link } from "react-router-dom";
import { Create } from "@/pages/department/Create";
import { IDepartment } from "@/types/department.type";

const columns: ColumnDef<IDepartment>[] = [
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
	},
	{
		accessorKey: "departmentId",
		header: () => {
			return <div>ID</div>;
		},
		cell: ({ row }) => (
			<Link
				to={row.getValue("departmentId")}
				className="cursor-pointer font-medium text-blue-600 underline"
			>
				{row.getValue("departmentId")}
			</Link>
		),
	},
	{
		accessorKey: "subjectName",
		header: "Tổ bộ môn",
		cell: ({ row }) => <div>{row.getValue("subjectName")}</div>,
	},
	{
		accessorKey: "description",
		header: "Mô tả",
		cell: ({ row }) => <div>{row.getValue("description")}</div>,
	},
	{
		accessorKey: "notification",
		header: "Thông báo",
		cell: ({ row }) => <div>{row.getValue("notification")}</div>,
	},
];

const data: IDepartment[] = [
	{
		departmentId: "lvbt123",
		subjectName: "Toán",
		description:
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit.  ",
		notification: "",
	},
	{
		departmentId: "tlvb321",
		subjectName: "Toán",
		description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
		notification: "",
	},
];
const Department = () => {
	const [selectedRow, setSelectedRow] = useState<IDepartment>();

	const handleChange = (value: IDepartment[]) => {
		setSelectedRow(value?.[0]);
	};

	const isDisableButton = !selectedRow;
	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ TỔ BỘ MÔN</div>
			<div className="mb-5">
				<div className="flex gap-2">
					<Create type="create" disable={false} />
					<Create
						type="edit"
						disable={isDisableButton}
						selected={selectedRow}
					/>
					<Button disabled={isDisableButton}>Xóa</Button>
				</div>
			</div>
			<div className="mb-5">
				<TableDetails
					data={data}
					columns={columns}
					onChange={handleChange}
					loading={false}
					useRadio
				/>
			</div>
		</>
	);
};

export default Department;
