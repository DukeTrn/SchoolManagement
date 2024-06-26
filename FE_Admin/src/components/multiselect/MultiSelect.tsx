// src/components/multi-select.tsx

import * as React from "react";
import { cva, type VariantProps } from "class-variance-authority";
import { CheckIcon, XCircle, ChevronDown } from "lucide-react";

import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
	Popover,
	PopoverContent,
	PopoverTrigger,
} from "@/components/ui/popover";
import {
	Command,
	CommandEmpty,
	CommandGroup,
	CommandInput,
	CommandItem,
	CommandList,
	CommandSeparator,
} from "@/components/ui/command";

const multiSelectVariants = cva(
	"m-1 transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 duration-300",
	{
		variants: {
			variant: {
				default:
					"border-foreground/10 text-foreground bg-card hover:bg-card/80",
				secondary:
					"border-foreground/10 bg-secondary text-secondary-foreground hover:bg-secondary/80",
				destructive:
					"border-transparent bg-destructive text-destructive-foreground hover:bg-destructive/80",
				inverted: "inverted",
			},
		},
		defaultVariants: {
			variant: "default",
		},
	}
);

interface MultiSelectProps
	extends React.ButtonHTMLAttributes<HTMLButtonElement>,
		VariantProps<typeof multiSelectVariants> {
	options: {
		label: string;
		value: string;
		icon?: React.ComponentType<{ className?: string }>;
	}[];
	onValueChange: (value: string[]) => void;
	value?: string[];
	placeholder?: string;
	animation?: number;
	maxCount?: number;
	asChild?: boolean;
	className?: string;
	width?: number;
	handleRetrieve?: () => void;
	errorMessage?: string;
}

export const MultiSelect = React.forwardRef<
	HTMLButtonElement,
	MultiSelectProps
>(
	(
		{
			options,
			onValueChange,
			variant,
			value = [],
			placeholder = "Select options",
			animation = 0,
			maxCount = 3,
			asChild = false,
			className,
			width,
			handleRetrieve,
			errorMessage,
			...props
		},
		ref
	) => {
		const [selectedValues, setSelectedValues] =
			React.useState<string[]>(value);
		const [isPopoverOpen, setIsPopoverOpen] = React.useState(false);
		const [isAnimating] = React.useState(false);

		React.useEffect(() => {
			if (JSON.stringify(selectedValues) !== JSON.stringify(value)) {
				setSelectedValues(value);
			}
		}, [value, selectedValues]);

		const handleInputKeyDown = (
			event: React.KeyboardEvent<HTMLInputElement>
		) => {
			if (event.key === "Enter") {
				setIsPopoverOpen(true);
			} else if (
				event.key === "Backspace" &&
				!event.currentTarget.value
			) {
				const newSelectedValues = [...selectedValues];
				newSelectedValues.pop();
				setSelectedValues(newSelectedValues);
				onValueChange(newSelectedValues);
			}
		};

		const toggleOption = (value: string) => {
			const newSelectedValues = selectedValues.includes(value)
				? selectedValues.filter((v) => v !== value)
				: [...selectedValues, value];
			setSelectedValues(newSelectedValues);
			onValueChange(newSelectedValues);
		};

		const handleTogglePopover = () => {
			setIsPopoverOpen((prev) => !prev);
		};

		return (
			<Popover open={isPopoverOpen} onOpenChange={setIsPopoverOpen}>
				<PopoverTrigger asChild>
					<Button
						ref={ref}
						{...props}
						onClick={handleTogglePopover}
						className={cn(
							"flex h-auto min-h-9 items-center justify-between rounded-md border bg-inherit p-[2px] hover:bg-inherit",
							className
						)}
						style={{ width: width ? `${width}px` : "100%" }}
					>
						{selectedValues.length > 0 ? (
							<div className="flex w-full items-center justify-between">
								<div className="flex flex-wrap items-center">
									{selectedValues
										.slice(0, maxCount)
										.map((value) => {
											const option = options.find(
												(o) => o.value === value
											);
											const IconComponent = option?.icon;
											return (
												<Badge
													key={value}
													className={cn(
														isAnimating
															? "animate-bounce"
															: "",
														multiSelectVariants({
															variant,
															className,
														})
													)}
													style={{
														animationDuration: `${animation}s`,
													}}
												>
													{IconComponent && (
														<IconComponent className="mr-2 h-4 w-4" />
													)}
													{option?.label}
													<XCircle
														className="ml-2 h-4 w-4 cursor-pointer"
														onClick={(event) => {
															event.stopPropagation();
															toggleOption(value);
														}}
													/>
												</Badge>
											);
										})}
									{selectedValues.length > maxCount && (
										<Badge
											className={cn(
												"border-foreground/1 bg-transparent text-foreground hover:bg-transparent",
												isAnimating
													? "animate-bounce"
													: "",
												multiSelectVariants({
													variant,
													className,
												})
											)}
											style={{
												animationDuration: `${animation}s`,
											}}
										>
											{`${selectedValues.length} selected`}
										</Badge>
									)}
								</div>
								{/* <div className="flex items-center justify-between">
									<XIcon
										className="mx-2 h-4 cursor-pointer text-muted-foreground"
										onClick={(event) => {
											event.stopPropagation();
											Promise.resolve()
												.then(() => handleClear())
												.then(
													() =>
														handleRetrive &&
														handleRetrive()
												);
										}}
									/>
									<Separator
										orientation="vertical"
										className="flex h-full min-h-6"
									/>
									<ChevronDown className="mx-2 h-4 cursor-pointer text-muted-foreground" />
								</div> */}
							</div>
						) : (
							<div className="mx-auto flex w-full items-center justify-between">
								<span className="mx-3 text-sm text-muted-foreground">
									{placeholder}
								</span>
								<ChevronDown className="mx-2 h-4 cursor-pointer text-muted-foreground" />
							</div>
						)}
					</Button>
				</PopoverTrigger>
				<PopoverContent
					className="w-[230px] p-0"
					align="start"
					onEscapeKeyDown={() => setIsPopoverOpen(false)}
				>
					<Command>
						<CommandInput
							placeholder="Search..."
							onKeyDown={handleInputKeyDown}
						/>
						<CommandList>
							<CommandEmpty>No results found.</CommandEmpty>
							<CommandGroup>
								{options.map((option) => {
									const isSelected = selectedValues.includes(
										option.value
									);
									return (
										<CommandItem
											key={option.value}
											onSelect={() =>
												toggleOption(option.value)
											}
											style={{
												pointerEvents: "auto",
												opacity: 1,
											}}
											className="cursor-pointer"
										>
											<div
												className={cn(
													"mr-2 flex h-4 w-4 items-center justify-center rounded-sm border border-primary",
													isSelected
														? "bg-primary text-primary-foreground"
														: "opacity-50 [&_svg]:invisible"
												)}
											>
												<CheckIcon className="h-4 w-4" />
											</div>
											{option.icon && (
												<option.icon className="mr-2 h-4 w-4 text-muted-foreground" />
											)}
											<span>{option.label}</span>
										</CommandItem>
									);
								})}
							</CommandGroup>
							<CommandSeparator />
							<CommandGroup>
								<div className="flex items-center justify-between">
									{/* {selectedValues.length > 0 && (
										<>
											<CommandItem
												onSelect={handleClear}
												style={{
													pointerEvents: "auto",
													opacity: 1,
												}}
												className="flex-1 cursor-pointer justify-center"
											>
												Clear
											</CommandItem>
											<Separator
												orientation="vertical"
												className="flex h-full min-h-6"
											/>
										</>
									)}
									<CommandSeparator /> */}
									<CommandItem
										onSelect={() => {
											setIsPopoverOpen(false);
											handleRetrieve && handleRetrieve();
										}}
										style={{
											pointerEvents: "auto",
											opacity: 1,
										}}
										className="flex-1 cursor-pointer justify-center"
									>
										OK
									</CommandItem>
								</div>
							</CommandGroup>
						</CommandList>
					</Command>
				</PopoverContent>
				{errorMessage && (
					<div className="mt-1 min-h-[1.25rem] text-sm text-red-600">
						{errorMessage}
					</div>
				)}
			</Popover>
		);
	}
);

MultiSelect.displayName = "MultiSelect";
