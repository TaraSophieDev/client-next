<template>
    <div
        class="chart-item"
        :class="isSpinShare ? 'is-spinshare' : 'is-local'"
        @click="handleClick"
        :tabindex="isSpinShare ? 0 : -1"
    >
        <div
            class="cover"
            :style="`background-image: url(${Cover})`"
        ></div>
        <div class="meta">
            <div class="title">{{ Title }}</div>
            <div class="artist">{{ Artist }} &bull; {{ Charter }}</div>
            <div class="difficulties">
                <span :class="{ active: EasyDifficulty }">
                    <span>E</span>
                    <span v-if="EasyDifficulty">{{ EasyDifficulty }}</span>
                </span>
                <span :class="{ active: NormalDifficulty }">
                    <span>N</span>
                    <span v-if="NormalDifficulty">{{ NormalDifficulty }}</span>
                </span>
                <span :class="{ active: HardDifficulty }">
                    <span>H</span>
                    <span v-if="HardDifficulty">{{ HardDifficulty }}</span>
                </span>
                <span :class="{ active: ExpertDifficulty }">
                    <span>EX</span>
                    <span v-if="ExpertDifficulty">{{ ExpertDifficulty }}</span>
                </span>
                <span :class="{ active: XDDifficulty }">
                    <span>XD</span>
                    <span v-if="XDDifficulty">{{ XDDifficulty }}</span>
                </span>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue';
import router from '@/router';

const props = defineProps({
    SpinShareReference: {
        type: [String, Boolean],
        default: false,
    },
    Title: {
        type: String,
        required: true,
    },
    Artist: {
        type: String,
        required: true,
    },
    Charter: {
        type: String,
        required: true,
    },
    Cover: {
        type: [String, Boolean],
        default: false,
    },
    EasyDifficulty: {
        type: [Number, Boolean],
        default: false,
    },
    NormalDifficulty: {
        type: [Number, Boolean],
        default: false,
    },
    HardDifficulty: {
        type: [Number, Boolean],
        default: false,
    },
    ExpertDifficulty: {
        type: [Number, Boolean],
        default: false,
    },
    XDDifficulty: {
        type: [Number, Boolean],
        default: false,
    },
});

const isSpinShare = computed(
    () =>
        props.SpinShareReference.includes('spinshare_') &&
        props.SpinShareReference.split(' ')[0].length === 23,
);

const handleClick = () => {
    if (isSpinShare.value) {
        router.push({
            path: '/chart/' + props.SpinShareReference.split(' ')[0],
        });
    }
};
</script>

<style lang="scss" scoped>
.chart-item {
    height: 85px;
    background: rgba(var(--colorBaseText), 0.07);
    border-radius: 6px;
    padding: 10px;
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 10px;
    align-items: center;
    transition: 0.15s ease-in-out all;

    & .cover {
        width: 65px;
        height: 65px;
        border-radius: 4px;
        background-position: center;
        background-size: cover;
    }
    & .meta {
        display: grid;
        gap: 3px;

        & .title,
        & .subtitle {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
        & .artist {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            color: rgba(var(--colorBaseText), 0.4);
            font-size: 0.9rem;
        }
        & .difficulties {
            display: flex;
            gap: 5px;
            margin-top: 5px;

            & > span {
                padding: 3px 7px;
                background: rgba(var(--colorBaseText), 0.07);
                border-radius: 2px;
                font-size: 0.6rem;

                & span:nth-child(1) {
                    font-weight: bold;
                }
                & span:nth-child(2) {
                    margin-left: 5px;
                }

                &:not(.active) {
                    opacity: 0.4;
                }
            }
        }
    }

    &:not(.is-local):hover {
        background: rgba(var(--colorBaseText), 0.14);
        cursor: pointer;
    }
    &.is-local {
        background: transparent;
        border: 1px solid rgba(var(--colorBaseText), 0.07);
    }
}
.ui-console .chart-item {
    height: 120px;
    border-radius: 4px;
    gap: 15px;

    & .cover {
        width: 100px;
        height: 100px;

        & .tag {
            height: 30px;
            width: 30px;
            font-size: 1.15rem;
        }
    }
    & .meta {
        gap: 5px;

        & .title {
            font-size: 1.05rem;
            font-weight: bold;
        }

        & .difficulties {
            gap: 10px;
            margin-top: 10px;

            & > span {
                padding: 6px 14px;
                border-radius: 3px;
                font-size: 0.75rem;
            }
        }
    }

    &:focus {
        outline: 3px solid rgba(var(--colorBaseText), 1);
    }
}
</style>
