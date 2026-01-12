"use client"

import * as React from "react"
import { ChevronDownIcon } from "lucide-react"

import { Button } from "@/components/ui/button"
import { Calendar } from "@/components/ui/calendar"
import { Input } from "@/components/ui/input"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"

type DateTimePickerProps = {
  value?: Date
  onChange: (value: Date | undefined) => void
  labelDate?: string
  labelTime?: string
}

export function DateTimePicker({
  value,
  onChange,
}: DateTimePickerProps) {
  const [open, setOpen] = React.useState(false)

  const date = value
  const timeValue = value
    ? value.toTimeString().slice(0, 8)
    : ""

  function handleDateSelect(selected?: Date) {
    if (!selected) {
      onChange(undefined)
      return
    }

    const base = value ?? new Date()
    const next = new Date(base)

    next.setFullYear(
      selected.getFullYear(),
      selected.getMonth(),
      selected.getDate()
    )

    onChange(next)
    setOpen(false)
  }

  function handleTimeChange(e: React.ChangeEvent<HTMLInputElement>) {
    if (!e.target.value) {
      onChange(undefined)
      return
    }

    const [h, m, s = "0"] = e.target.value.split(":")
    const base = value ?? new Date()
    const next = new Date(base)

    next.setHours(Number(h), Number(m), Number(s), 0)
    onChange(next)
  }

  return (
    <div className="flex gap-4">
      <div className="flex flex-col gap-3">
        <Popover open={open} onOpenChange={setOpen}>
          <PopoverTrigger asChild>
            <Button
              variant="outline"
              className="w-32 justify-between font-normal"
            >
              {date ? date.toLocaleDateString() : "Select date"}
              <ChevronDownIcon />
            </Button>
          </PopoverTrigger>

          <PopoverContent className="w-auto p-0" align="start">
            <Calendar
              mode="single"
              selected={date}
              captionLayout="dropdown"
              onSelect={handleDateSelect}
            />
          </PopoverContent>
        </Popover>
      </div>

      <div className="flex flex-col gap-3">
        <Input
          type="time"
          step="1"
          value={timeValue}
          onChange={handleTimeChange}
          className="bg-background appearance-none [&::-webkit-calendar-picker-indicator]:hidden"
        />
      </div>
    </div>
  )
}
