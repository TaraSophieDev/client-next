<template>
    <div
        @click="emit('click')"
        @keyup.enter="emit('click')"
        class="spin-tab-item"
        :class="{ active: active }"
        tabindex="0"
    >
        <slot />
    </div>
</template>

<script setup>
const emit = defineEmits(['click']);
defineProps({
    active: {
        type: Boolean,
        default: false,
    },
});
</script>

<style lang="scss" scoped>
.spin-tab-item {
    padding: 15px 15px 0;
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 10px;
    color: rgba(var(--colorBaseText), 0.6);
    transition: 0.15s ease-in-out background, 0.15s ease-in-out color;
    overflow: hidden;

    &:hover {
        background: rgba(var(--colorBaseText), 0.07);
        cursor: pointer;
    }
    &::after {
        content: '';
        display: block;
        width: 50%;
        min-width: 50px;
        height: 3px;
        background: transparent;
        border-radius: 50px;
        transition: 0.15s ease-in-out transform;
        transform: translateY(4px);
    }
    &.active {
        color: rgba(var(--colorBaseText), 1);

        &::after {
            background: rgb(var(--colorPrimary));
            transform: translateY(0);
        }
    }
}

.ui-console .spin-tab-item {
    padding: 12px 25px;
    border-radius: 100px;
    font-weight: bold;
    text-transform: uppercase;

    &::after {
        all: unset;
    }

    &:hover {
        background: rgba(var(--colorBaseText), 0.07);
    }
    &.active {
        background: rgba(var(--colorBaseText), 1);
        color: rgba(var(--colorBase), 1);
    }

    &:focus {
        outline: 3px solid rgba(var(--colorBaseText), 1);
    }
}
</style>
