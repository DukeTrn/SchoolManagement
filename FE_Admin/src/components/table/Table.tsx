import React, { useEffect } from "react";
import {
	ColumnDef,
	ColumnFiltersState,
	SortingState,
	VisibilityState,
	flexRender,
	getCoreRowModel,
	getFilteredRowModel,
	getPaginationRowModel,
	getSortedRowModel,
	useReactTable,
} from "@tanstack/react-table";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";

import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from "@/components/ui/table";

interface IProps {
	onChange?: (value: any) => void;
	columns: ColumnDef<any>[];
	data: any;
	loading?: boolean;
	useRadio?: boolean;
}
export function TableDetails(props: IProps) {
	const {
		onChange,
		columns,
		data,
		loading = false,
		useRadio = false,
	} = props;
	const [sorting, setSorting] = React.useState<SortingState>([]);
	const [columnFilters, setColumnFilters] =
		React.useState<ColumnFiltersState>([]);
	const [columnVisibility, setColumnVisibility] =
		React.useState<VisibilityState>({});
	const [rowSelection, setRowSelection] = React.useState({});

	const table = useReactTable({
		data,
		columns,
		onSortingChange: setSorting,
		onColumnFiltersChange: setColumnFilters,
		getCoreRowModel: getCoreRowModel(),
		getPaginationRowModel: getPaginationRowModel(),
		getSortedRowModel: getSortedRowModel(),
		getFilteredRowModel: getFilteredRowModel(),
		onColumnVisibilityChange: setColumnVisibility,
		onRowSelectionChange: setRowSelection,
		enableMultiRowSelection: !useRadio,
		enableSubRowSelection: !useRadio,
		initialState: {
			pagination: {
				pageSize: 10,
			},
		},
		state: {
			sorting,
			columnFilters,
			columnVisibility,
			rowSelection,
		},
	});

	const selectedRows = table
		.getFilteredSelectedRowModel()
		.flatRows.map((item) => item.original);

	useEffect(() => {
		onChange && onChange(selectedRows);
	}, [rowSelection]);

	useEffect(() => {
		table.setPageSize(data?.length);
	}, [data?.length]);

	useEffect(() => {
		table.resetRowSelection();
	}, [loading]);

	return (
		<div className="w-full">
			<div className="rounded-md border">
				<Table
					className="relative overflow-y-auto"
					// style={{ width: table.getCenterTotalSize() }}
				>
					<TableHeader>
						{table.getHeaderGroups().map((headerGroup) => (
							<TableRow key={headerGroup.id}>
								{headerGroup.headers.map((header) => {
									return (
										<TableHead key={header.id}>
											{header.isPlaceholder
												? null
												: flexRender(
														header.column.columnDef
															.header,
														header.getContext()
												  )}
										</TableHead>
									);
								})}
							</TableRow>
						))}
					</TableHeader>
					<TableBody>
						{table.getRowModel().rows?.length ? (
							table.getRowModel().rows.map((row) => (
								<TableRow
									key={row.id}
									data-state={
										row.getIsSelected() && "selected"
									}
								>
									{row.getVisibleCells().map((cell) => {
										return loading ? (
											<TableCell
												key={cell.id}
												style={{
													width: cell.column.getSize(),
												}}
											>
												<Skeleton className="h-[15px] w-full rounded bg-gray-200" />
											</TableCell>
										) : (
											<TableCell
												key={cell.id}
												style={{
													width: cell.column.getSize(),
												}}
											>
												{flexRender(
													cell.column.columnDef.cell,
													cell.getContext()
												)}
											</TableCell>
										);
									})}
								</TableRow>
							))
						) : (
							<TableRow>
								<TableCell
									colSpan={columns.length}
									className="h-24 text-center"
								>
									Không có dữ liệu
								</TableCell>
							</TableRow>
						)}
					</TableBody>
				</Table>
			</div>
		</div>
	);
}
