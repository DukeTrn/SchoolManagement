import { useEffect, useState } from "react";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { TableDetails } from "@/components/table/Table";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "@/components/ui/checkbox";
import { useDebounce } from "@/hooks";
import { useToast } from "@/components/ui/use-toast";
import Create from "./Create";

import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Link } from "react-router-dom";
import { ISubject } from "@/types/assignment.type";
import { deleteSubject, getSubjects } from "@/apis/asignment.api";

const Assignment = () => {
	const { toast } = useToast();
	const [classes, setClasses] = useState<ISubject[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [searchValue, setSearchValue] = useState("");
	const [selectedRow, setSelectedRow] = useState<ISubject>();
	const [selectedField, setSelectedField] = useState<string>("10");

	const searchQuery = useDebounce(searchValue, 1500);

	const isDisableButton = !selectedRow;

	const columns: ColumnDef<ISubject>[] = [
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
			accessorKey: "subjectName",
			header: "Môn học",
			cell: ({ row }) => {
				const id = row.original.id;

				return (
					<Link
						to={String(id)}
						state={{ ...row.original, selectedField }}
						className="cursor-pointer font-medium text-blue-600 underline"
					>
						{row.getValue("subjectName")}
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
		},
		{
			accessorKey: "description",
			header: "Mô tả",
			cell: ({ row }) => <div>{row.getValue("description")}</div>,
			size: 600,
		},
	];

	useEffect(() => {
		handleGetData();
	}, [searchQuery, selectedField]);

	const handleChange = (value: ISubject[]) => {
		setSelectedRow(value?.[0]);
	};

	const handleGetData = () => {
		setLoading(true);
		getSubjects(Number(selectedField)).then((res) => {
			setLoading(false);
			setClasses(res?.data?.data);
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
		deleteSubject(selectedRow?.id!)
			.then(() => {
				refreshData("Xóa môn học thành công!");
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
			<div className="mb-4 text-2xl font-medium">
				QUẢN LÝ PHÂN CÔNG GIẢNG DẠY
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
						selected={selectedRow}
						refreshData={refreshData}
					/>
					<Button disabled={isDisableButton} onClick={handleDelete}>
						Xóa
					</Button>
				</div>
			</div>
			<div>
				<TableDetails
					pageSize={10}
					data={classes}
					columns={columns}
					onChange={handleChange}
					loading={loading}
					useRadio
				/>
			</div>
		</>
	);
};

export default Assignment;
