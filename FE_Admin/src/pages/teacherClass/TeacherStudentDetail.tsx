import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import Pagination from "@/components/pagination";
import { ColumnDef } from "@tanstack/react-table";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import { Create } from "./Create";
import { useToast } from "@/components/ui/use-toast";
import { getTeacherStudentDetail } from "@/apis/teacher.info.api";

export default function TeacherStudentDetail() {
	const location = useLocation();
	const navigation = useNavigate();
	const { toast } = useToast();
	const state = location?.state;
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [student, setStudent] = useState<any>([]);
	const [selectedRow, setSelectedRow] = useState<any>();
	const semester = state.semester;

	const columns: ColumnDef<any>[] = [
		{
			accessorKey: "subjectName",
			header: () => {
				return <div>Môn học</div>;
			},
			cell: ({ row }) => <div>{row.getValue("subjectName")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "weight1",
			header: () => {
				return <div>ĐĐGtx</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight1") as any[];
				return (
					<div>
						{weights?.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
			minSize: 200,
		},
		{
			accessorKey: "weight2",
			header: () => {
				return <div>ĐĐGgk</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight2") as any[];
				return (
					<div>
						{weights?.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
		},
		{
			accessorKey: "weight3",
			header: () => {
				return <div>ĐĐGck</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight3") as any[];
				return (
					<div>
						{weights?.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
		},
	];

	const handleChange = (value: any[]) => {
		setSelectedRow(value?.[0]);
	};

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize]);

	const handleGetData = () => {
		setLoading(true);
		getTeacherStudentDetail(
			state.grade,
			semester,
			state.classDetailId,
			state.subjectId
		).then((res) => {
			setStudent(res.data?.data);
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

	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">{`KẾT QUẢ HỌC TẬP HỌC SINH ${state.fullName.toUpperCase()} `}</div>
			</div>
			<div className="mb-5">
				<div className="flex gap-2">
					<Create
						type="create"
						disable={false}
						refreshData={refreshData}
						items={state}
						student={student}
					/>
					<Create
						type="edit"
						disable={false}
						selected={selectedRow}
						refreshData={refreshData}
						items={state}
						student={student}
					/>
				</div>
			</div>

			<div>
				<TableDetails
					pageSize={pageSize}
					data={[student]}
					columns={columns}
					onChange={handleChange}
					loading={loading}
				/>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					Chú Thích
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGtx: Điểm Đánh Giá thường xuyên
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGgk: Điểm Đánh Giá giữa kỳ
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGck: Điểm Đánh Giá cuối kỳ
				</div>
			</div>
		</>
	);
}
